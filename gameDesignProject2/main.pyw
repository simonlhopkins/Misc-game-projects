import pygame,Puzzle, time, Player,bot, sys

#you need tweepy and pygame
#random.seed(2)
class Timer():
    def __init__(self, secs):
        pygame.font.init()
        self.currentTime= time.time()
        self.font=pygame.font.Font("data-latin.ttf", 30)
        self.text=secs
        self.textSurf= self.font.render(str(self.text), 1, (0,255,0))
    def draw(self, surface):
        self.incrimentTime()
        surface.blit(self.textSurf, (570,0))
    def incrimentTime(self):
        if(time.time()-self.currentTime>1 and self.text!=0):
            self.currentTime=time.time()
            self.text -= 1
            self.textSurf = self.font.render(str(self.text), 1, (0, 255, 0))


class Text():
    def __init__(self, text):
        self.font = pygame.font.Font("data-latin.ttf", 20)
        self.text = text
        self.time=0
        #self.textSurf = self.font.render(self.text, 1, (0, 255, 0))
        self.textSurf=pygame.Surface((0,0))
        self.currentChar=0
    def letterByLetter(self, des, pos):
        if(self.currentChar<=len(self.text)):
            self.textSurf=self.font.render(self.text[:self.currentChar], 1, (0, 255, 0))
            self.time+=1
            if self.time%3==0:
                self.currentChar+=1
        des.blit(self.textSurf, pos)
    def drawComplete(self, des, y):
        self.textSurf=self.textSurf=self.font.render(self.text, 1, (0, 255, 0))
        des.blit(self.textSurf, (0, y))


class Game():
    def __init__(self, player):
        self.currentPuzzle=None
        self.taskText=""
        self.timer=Timer(0)
        self.puzzles= dict()
        self.gameStep=-1
        self.gameScript=open("gameScript.txt").read().split("\n")
        self.currentY=-20
        self.twitterBot= bot.Bot()
        self.currentText=Text(self.gameScript[self.gameStep])
        self.textList = [[self.currentText, self.currentY]]
        self.puzzleFailed=False
        self.puzzleDif=0
        self.lineData=self.gameScript[0]
        self.keyPresses=0
        self.bonusTime=0
        self.player=player
    def incrementGameStep(self):
        self.gameStep += 1
        self.lineData=self.gameScript[self.gameStep].split("|")
        if len(self.lineData) != 2:
            self.puzzleFailed = False


        if self.currentPuzzle!=None:
            return
        elif "*" in self.lineData[int(self.puzzleFailed)]:
            return

        if self.currentY>200:
            for i in range(0, len(self.textList)):
                self.textList[i][1]=self.textList[i][1]-20
        else:
            self.currentY += 20
        self.currentText=Text(self.lineData[self.puzzleFailed])
        self.textList.append([self.currentText, self.currentY])

    def drawGameText(self, des):
        if self.lineData[0][:6]=="*level":
            self.currentY = 0
            self.textList = []
            self.currentPuzzle=self.puzzles[self.lineData[self.puzzleDif][1:len(self.lineData[self.puzzleDif])]]
            self.currentPuzzle.draw(des, (0, self.currentY))
            self.timer = Timer(int(self.lineData[len(self.lineData)-1]))
            self.incrementGameStep()
            self.puzzleDif = 0
        elif self.lineData[int(self.puzzleFailed)][:6] == "*post*":
            print("post")
            textToPost=self.lineData[int(self.puzzleFailed)][6:]
            print(textToPost)
            if "USER" in textToPost:
                textToPost=textToPost.replace("USER", "@"+self.player.userName)
            if "RANDOM" in textToPost:
                textToPost=textToPost.replace("RANDOM", self.twitterBot.getRandomUser())
            self.twitterBot.post(textToPost)
            #self.puzzleFailed = False
            self.incrementGameStep()
            return
        elif (self.lineData[int(self.puzzleFailed)] == "*changeUserProfile*"):
            self.twitterBot.changeUserProfile(self.player.userName)
            #self.puzzleFailed = False
            self.incrementGameStep()
            return
        elif (self.lineData[int(self.puzzleFailed)][:len("*sendDirectMessage*")] == "*sendDirectMessage*"):
            self.twitterBot.sendDirectMessage(self.player.userName, self.lineData[int(self.puzzleFailed)][len("*sendDirectMessage*"):])
            #self.puzzleFailed = False
            self.incrementGameStep()
            return
        elif (self.lineData[int(self.puzzleFailed)][:len("*readMostRecentDirectMessages*")] == "*readMostRecentDirectMessages*"):
            response=self.twitterBot.readMostRecentDirectMessages(self.lineData[int(self.puzzleFailed)][len("*readMostRecentDirectMessages*"):])
            if response==True:
                self.bonusTime+=20
            #self.puzzleFailed = False
            self.incrementGameStep()
            return
        elif (self.lineData[int(self.puzzleFailed)][:len("*changeProfileToGreenText*")] == "*changeProfileToGreenText*"):
            self.twitterBot.changeProfileToGreenText()
            # self.puzzleFailed = False
            self.incrementGameStep()
            return
        elif (self.lineData[int(self.puzzleFailed)]=="*quit*"):
            print("finished")
            timeline = self.twitterBot.api.user_timeline(count=10)
            self.twitterBot.api.update_profile_image(filename="defaultProfilePic.png")
            self.twitterBot.api.update_profile(name="bot")
            for i in timeline:
                self.twitterBot.api.destroy_status(i.id)
            sys.exit()
        elif self.lineData[int(self.puzzleFailed)] == "*s":
            self.incrementGameStep()
        elif self.lineData[int(self.puzzleFailed)] == "*":
            return
        else:
            self.currentText.letterByLetter(des, (0, self.currentY))
            for i in range(0, len(self.textList) - 1):
                self.textList[i][0].drawComplete(des, self.textList[i][1])
            #self.puzzleFailed = False





    def update(self):
        if self.currentPuzzle!=None:
            if self.currentPuzzle.testIfSolved()==True:
                self.puzzleFailed = False
                self.textList = []
                self.currentY = -20
                self.currentPuzzle = None

                self.timer=Timer(0)
                self.incrementGameStep()
                print("puzzle is solved")
        if self.timer.text==0 and self.currentPuzzle!=None:
            self.currentY=-20
            self.textList = []
            self.puzzleFailed=True
            self.currentPuzzle=None
            self.incrementGameStep()








#game.currentPuzzle=game.puzzles["level1Puzzle"]









def main():
    # Initialise screen

    player = Player.Player(input("Enter Twitter User Name: "))
    print("loading...")
    game = Game(player)

    game.puzzles["level1Puzzle"] = Puzzle.Puzzle(3, 3)
    game.puzzles["level2Puzzle"] = Puzzle.Puzzle(5, 5)
    game.puzzles["level3Puzzle"] = Puzzle.Puzzle(10, 10)
    game.puzzles["level4Puzzle"] = Puzzle.Puzzle(3, 3)
    game.puzzles["level5Puzzle"] = Puzzle.Puzzle(18, 18)
    game.puzzles["level6Puzzle"] = Puzzle.Puzzle(18, 18)
    game.puzzles["level7Puzzle"] = Puzzle.Puzzle(4, 4)
    game.puzzles["level8Puzzle"] = Puzzle.Puzzle(5, 5)
    # game.puzzles["level6Puzzle"] = Puzzle.Puzzle(18, 18)


    def clearTimeline():
        timeline = game.twitterBot.api.user_timeline(count=10)
        game.twitterBot.api.update_profile_image(filename="defaultProfilePic.png")
        game.twitterBot.api.update_profile(name="bot")
        for i in timeline:
            game.twitterBot.api.destroy_status(i.id)
    clearTimeline()


    # Fill background
    pygame.init()
    screen = pygame.display.set_mode((600, 600))
    background = pygame.Surface(screen.get_size())
    background = background.convert()
    background.fill((250, 250, 250))
    if game.currentPuzzle!=None:
        game.currentPuzzle.draw(background, (game.currentY,0))

    # Blit everything to the screen
    screen.blit(background, (0, 0))
    pygame.display.flip()
    clock = pygame.time.Clock()
    game.incrementGameStep()
    #adds 2 for some reason
    # Event loop
    while 1:
        clock.tick(30)
        #events = pygame.event.get()

        for event in pygame.event.get():
            if event.type==pygame.MOUSEBUTTONDOWN:
                if game.currentPuzzle!=None:
                    for square in game.currentPuzzle.tileList:
                        square[0].rotate()
            if event.type == pygame.QUIT:
                return
            if event.type == pygame.KEYDOWN:
                if game.currentPuzzle==None:
                    game.incrementGameStep()
        screen.blit(background, (0, 0))
        background.fill((0, 0, 0))
        if game.currentPuzzle!=None:
            #probably should make this a function in puzzle update
            for square in game.currentPuzzle.tileList:
                square[0].update()

            game.currentPuzzle.drawSolution(background)
            game.currentPuzzle.update()
            game.timer.draw(background)
        game.drawGameText(background)
        game.update()



        #pygame.display.update()
        pygame.display.flip()


if __name__ == '__main__': main()