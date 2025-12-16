# ITD_DDA_ASG
ASG for ITD and DDA

**ITD**

Controls: Scan the image with your app, tap on the screen to interact, physically move around to move
To use, download the app, and launch it on your device

How to play: After loggin in or signing up, the user's ID will be indicated in the realtime database and their scores will be recorded in firebase itself to be stored as their leaderboard. Players can then choose their pet (hamster) and scan the image of the hamster's cage to make the hamster pop up. Players can feed and learn more about how to take care of the pet properly throughout the game. 

Answer Key for Quiz: 
Qn: 
Ans:

Platform/Hardware needed: Unity & Firebase, available on Mobile Android. Camera needed.

Limitations/Bugs:
-might encounter some issues with the sign up login page as the connection between the ar scene and the regular scene seems to be overlapping.
-some ui are quite unresponsive. 
-we only limit to having 1 pet available, which is the hamster as the main choice.


References:
textures for assets from: https://milkandbanana.itch.io/gradient-color-pallete
fonts for words and buttons displayed: https://fonts.google.com/specimen/Jua


**DDA**
  What content you are displaying:
  -Raising a virtual pet
  -Interacting with the pet
    -feeding, growing
  -getting scores from quizzes and raising your pet properly
    -featured on leaderboard that is linked to firebase
    
• What is the application catering for:
-Players that are unsure of whether they want to keep a real pet or are unsure if they are capable enough to raise a real pet.
-This is the starting base for them to experience it first hand without harming real animals.

• Documented Wireframes/game flow:
  1: game start - logo, start, how to play, leaderboard (link to web?)
  2: choose pets -  hamster, cat, dog (both cat and dog unavailable)
  3: Text - please scan the home of your pet(?) - pet shows up
  4: (2nd layer 1st) - info on the hamster (prt) button that writes “im ready now?”
  5: affection bar shows up as well as a hamburger menu after ui in 4 is closed
  6: inside the hamburger menu 2 sections shows clearly what the pet is allowed to eat
  -can eat
  Image of food + buttons -use, info
  -cannot eat
  Also gives info on why the pet is not allowed to eat it and allows the user to use it to “kill” the pet
  7: Death screen top writes game over while two buttons appear on the bottom. Why and retry. 
  Why gives information about why the pet died + retry brings you back to the homescreen
  8: 4th on 1st row- info on why pet died 
  
• State & Attribute all external assets/libraries used

Original artwork/assets:
Models:
Almond
Caffine
Broccoli
Hamster
Strawberry
Onion
Sunflower Seed
Carrot
