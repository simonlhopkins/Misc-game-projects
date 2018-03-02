import tweepy, urllib, random, time
class Bot():
    def __init__(self):
        self.ckey = '3kbiPoIUFIAaosO9VpqvC23DX'
        self.csecret = 'jdwetK7IzP0aO8AsBQO0CvYXlFYFBMobN25g6Nl7vxHwn7t5Oo'
        self.atoken = '845319842559397890-GU0JUUSkTbNgUigZCUfcwN88Pm6Joui'
        self.asecret = 'LB3Zx81THLirIlBzy70Neq5FF91VkjBS3xvIXS0jHlSRt'

        self.auth = tweepy.OAuthHandler(self.ckey, self.csecret)
        self.auth.set_access_token(self.atoken, self.asecret)
        self.api = tweepy.API(self.auth)
        self.followerList=[]
        for i in tweepy.Cursor(self.api.followers).items():
            try:
                self.followerList.append(i)
                time.sleep(1)
            except tweepy.TweepError:
                break
    def post(self, text):
        self.api.update_status(status=text)
    def changeUserProfile(self, targetUser):
        urllib.request.urlretrieve(self.api.get_user(screen_name=targetUser).profile_image_url, "userProfilePic.png")
        self.api.update_profile_image(filename="userProfilePic.png")
        self.api.update_profile(name=self.api.get_user(screen_name=targetUser).name)
    def changeProfileToGreenText(self):
        self.api.update_profile_image(filename="evil.png")
        self.api.update_profile(name="Mr. Greentext")
    def getRandomUser(self):
        if len(self.followerList)>0:
            print(self.followerList[random.randint(0,len(self.followerList)-1)].screen_name)
            return self.followerList[random.randint(0,len(self.followerList)-1)].screen_name
    def sendDirectMessage(self, recepient, words):
        self.api.send_direct_message(screen_name=recepient, text=words)
    def readMostRecentDirectMessages(self, expectedResponse):
        for i in tweepy.Cursor(self.api.direct_messages).items():
            if i.text.upper()==expectedResponse.upper():
                return True
            else:
                return False