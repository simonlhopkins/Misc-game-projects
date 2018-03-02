import pygame, random

def generatePuzzle(width, height):
    #this will create a 2D array of the a puzzle that another method will take in and draw
    blockTypes=["lBlock", "xBlock", "iBlock"]
    puzzleArray=[]
    for i in range(0, height):
        puzzleArray.append([])
        for j in range(0, width):
            puzzleArray[i].append(blockTypes[random.randint(0, len(blockTypes)-1)])
    return puzzleArray

class puzzleBlock():
    def __init__(self, x,y,type, des, puzzle):
        self.puzzle=puzzle
        self.type=type
        self.des=des
        self.clicked=False
        '''rotation is either 1,2,3,4
        1   ->  No rotation, the nodes should be the same as when they started
        2   ->  Rotated 90
        3   ->  Rotated 180
        4   ->  Rotated 270
        '''
        self.rotation=random.randint(1,4)
        self.rect= pygame.Rect(x, y, 30, 30)
        self.image = pygame.image.load(self.type + ".png")
        #self.puzzle.tileList.append(self)
        self.inputNode=0
        self.outputNode=0
        self.nodes=[0,0]
        if(self.type=="lBlock"):
            self.nodes=[1,2]
        elif(self.type=="iBlock"):
            self.nodes=[1,3]
        elif (self.type == "endPointBlock"):
            self.nodes = [3]
        elif (self.type == "startPointBlock"):
            self.nodes = [3]
        elif (self.type == "xBlock"):
            self.nodes = [1,2,3,4]
        else:
            self.nodes=[0,0]

        #if self.type!="obBlock":
        #    self.nodes=[self.nodes[0]+self.rotation, self.nodes[1]+self.rotation]
        #for i in range(0, len(self.nodes)):
        #    if(self.nodes[i]>4):
        #        self.nodes[i]= self.nodes[i]-4


        #self.image = pygame.transform.rotate(self.image, ((self.rotation - 1) * 90))
        self.image = pygame.transform.scale(self.image, (30, 30))

    def update(self):
        self.des.blit(self.image, (self.rect.x, self.rect.y))

    def rotate(self):
        if(self.rect.collidepoint(pygame.mouse.get_pos())):
            if(self.rotation<4):
                self.rotation+=1
            else:
                self.rotation=1
            for i in range(0, len(self.nodes)):
                self.nodes[i]+=1
                if (self.nodes[i] > 4):
                    self.nodes[i] = 1
            self.image = pygame.transform.rotate(self.image, -90)
            if self.inputNode!=0:
                self.inputNode+=1
                if self.inputNode>4:
                    self.inputNode=1
            if self.outputNode!=0:
                self.outputNode+=1
                if self.outputNode > 4:
                    self.outputNode = 1

#the actual puzzle, methods to draw the entire puzzle
class Puzzle():
    def __init__(self, width, height):
        self.width=width
        self.height=height
        self.puzzle=generatePuzzle(self.width,self.height)
        self.solution=self.makeSolution(self.puzzle)
        self.pathList=[]*len(self.puzzle)
        self.tileList=[]
        self.solvedList=[]
        self.isSolved=False
        self.currentPos = 0
        self.pos=(0,0)
        self.currentBlock=self.getBlockWithCoord(self.solution[0])
        for i in range(0, len(self.solution)):
            if i==0 :
                self.puzzle[self.solution[i][1]][self.solution[i][0]] = "startPointBlock"
                continue
            if i==len(self.solution)-1:
                self.puzzle[self.solution[i][1]][self.solution[i][0]]= "endPointBlock"
                continue
            #strait block
            #same row
            if(self.solution[i-1][0]== self.solution[i+1][0]):
                self.puzzle[self.solution[i][1]][self.solution[i][0]]="iBlock"
            #same column
            elif (self.solution[i - 1][1] == self.solution[i + 1][1]):
                self.puzzle[self.solution[i][1]][self.solution[i][0]] = "iBlock"

            else:
                self.puzzle[self.solution[i][1]][self.solution[i][0]] = "lBlock"
        for i in range(0, len(self.puzzle)):
            for j in range(0,len(self.puzzle[i])):
                index=[i,j]
                if(index not in self.solution):
                    if(random.randint(0,2)==1):
                        self.puzzle[j][i]="obBlock"
    def findPosSpots(self, pos):
        array=self.puzzle
        while (pos[0] != len(array) - 1):
            spots = []
            tempSpots = []
            tempSpots.append([pos[0] + 1, pos[1]])
            tempSpots.append([pos[0] - 1, pos[1]])
            tempSpots.append([pos[0], pos[1] + 1])
            tempSpots.append([pos[0], pos[1] - 1])
            for i in range(0, len(tempSpots)):
                if (tempSpots[i][0] >= 0 and tempSpots[i][0] < len(array[0])):
                    # spots.append(tempSpots[i])
                    if (tempSpots[i][1] >= 0 and tempSpots[i][1] < len(array)):
                        spots.append(tempSpots[i])
            return spots

    #works
    def makeSolution(self, array):
        currentPos = [0, random.randint(0, (len(array[0]) - 1))]
        startingPos = currentPos
        path = [startingPos]
        while (currentPos[0] != len(array) - 1):
            tSpots=self.findPosSpots(tuple(currentPos))
            spots=[]
            for i in range(0, len(tSpots)):
                if(tSpots[i] not in path):
                    spots.append(tSpots[i])
            if (spots == []):
                currentPos = [0, random.randint(0, (len(array[0]) - 1))]
                startingPos = currentPos
                # currentPos=[0,0]
                path = [startingPos]
                continue
            path.append(spots[random.randint(0, len(spots) - 1)])
            currentPos = path[len(path) - 1]

        return path

    def draw(self, destination, pos):
        self.pos=pos
        for i in range(0, len(self.puzzle)):
            for j in range(0, len(self.puzzle[i])):
                block=puzzleBlock(pos[0]+(i * 30), pos[1]+(j * 30), self.puzzle[i][j], destination, self)
                self.tileList.append((block, [j,i]))
        self.currentBlock = self.getBlockWithCoord(self.solution[0])
        self.solvedList.append(self.currentBlock)

    def drawSolution(self, destination):
        pressed=pygame.key.get_pressed()
        if(pressed[pygame.K_SPACE]):
            for i in range(0, len(self.puzzle)):
                for j in range(0, len(self.puzzle[i])):
                    if ([j, i] in self.solution):
                        testSurf = pygame.Surface((30, 30))
                        testSurf.set_alpha(128)
                        testSurf.fill((255, 0, 0))
                        destination.blit(testSurf, (i * 30, j * 30))
            #self.seeIfSolved(self.getBlockWithCoord([1, 1]))

            #self.seeIfSolved(self.getBlockWithCoord(self.solution[0]))
            #print(self.getBlockWithCoord([1,0]))
            #self.testIfSolved()




    def getBlockWithCoord(self, coord):
        for i in range(0, len(self.tileList)):
            if(coord in self.tileList[i]):
                return self.tileList[i]
        return False
    def testIfSolved(self):
        if self.tileList!=[]:
            self.solvedList=[self.currentBlock]
            test=self.getConnectedSquares(self.currentBlock)
            if test:
                return True
            else:
                return False

        #print(self.solvedList)

    def getConnectedSquares(self, targetBlock):
        for i in range(0, len(targetBlock[0].nodes)):

            if targetBlock[0].nodes[i]==1:
                tBlock=self.getBlockWithCoord([targetBlock[1][0]-1, targetBlock[1][1]])
            elif targetBlock[0].nodes[i] == 2:
                tBlock = self.getBlockWithCoord([targetBlock[1][0], targetBlock[1][1]+1])
            elif targetBlock[0].nodes[i] == 3:
                tBlock = self.getBlockWithCoord([targetBlock[1][0]+1, targetBlock[1][1]])
            elif targetBlock[0].nodes[i] == 4:
                tBlock = self.getBlockWithCoord([targetBlock[1][0], targetBlock[1][1]-1])
            else:
                continue

            #print(str([targetBlock[1][0], targetBlock[1][1]-1]))
            if tBlock==False:
                continue
            for j in range(0, len(tBlock[0].nodes)):
                if(tBlock[0].nodes[j]==0):
                    continue
                if(abs(targetBlock[0].nodes[i]-tBlock[0].nodes[j])==2):
                    if tBlock not in self.solvedList:
                        self.solvedList.append(tBlock)
                        #self.solvedList.append(targetBlock)
                        self.getConnectedSquares(tBlock)
                        testSurf = pygame.Surface((30, 30))
                        testSurf.set_alpha(50)
                        testSurf.fill((0, 255, 0))
                        tBlock[0].des.blit(testSurf, (self.pos[0]+(tBlock[1][1] * 30), self.pos[1]+tBlock[1][0] * 30))

        if self.getBlockWithCoord(self.solution[len(self.solution)-1]) in self.solvedList:
            return True
                    #print(self.solution[0])
                    #print(self.solution[len(self.solution)-1])
            #print(self.solvedList)



        return False

    def update(self):
        self.testIfSolved()