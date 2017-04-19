--IMPORTANT--
This is the repository of the laser project showcased on YouTube. I think it goes without saying, and was evident by my multiple winces on camera, that no one should shoot a laser into their eye. No one should use this technology to inflict bodily harm on themselves or anyone else. That being said, enjoy the repository.

--Personal Note-- 
To be honest, this project started as a joke, but quickly evolved into a personal challenge of ability. I had to tackle a lot of different problems and am
very pleased with the overall outcome.


--Summary--
This software processes incoming video from a live camera and searches for faces or human figures. If a detection is made, the system outputs coordinate data via serial port(1),
which is interpreted by additional software compiled on a microcontroller. The data is parsed and signals are sent to 2 servos arranged in such a way that one carries out the X
value and the other carries out the Y value of the coordinates. A laser is attached to the Y servo so that it points to the position of the detected object, ultimately shining a
laser in your face.


There is additional functionality of the system accessing and playing back sound files dependent on the situation. The software also constantly listens for voice commands and
carries out actions based on them.


1 - The coordinate data must be calculated based on the camera's current field of view. To avoid any hard-coding specific to my hardware, I created a calibration mode. In this
mode, the user uses arrow buttons to position the laser to the bounds of the screen on all 4 sides, these values are serialized to a json file (using json .net)(it's also used
to store settings) located in the same directory as the .exe. When the main detection mode is started and detects an object, outputs to the robot are scaled based on these values.


--Materials--
-OS Capable of compiling and running executable files, recommend Windows, but the same results may be achievable with a system running a .Net equivalent such as Mono (not tested)
-Webcam, I used the Logitech 9000 Pro, but it normally retails for around $110 and I got mine used for $20 on Amazon. I would not spend the full $110 on a webcam, any will do.
-ATmega 328 microcontroller mounted on an arduino board, but really any ATMEL microcontroller capable of PWM should do the trick
-2X Proster MG996R Digital Metal Copper Gear High Torque Servo Motors
-WYHP Mini Laser Dot Diode Module Head WL Red 650nm 6mm 5V 5mW. You can do with one, but I bought 10, only $2.66. May also consider something to mount it in, I used a hollow pen
-3X 5v 2a power supplies, 2 for the servos, 1 for the laser


--Dependencies--
-Emgu CV image recognition library v 2.4
-.Net Framework 4.5
-Telerik aesthetic framework (I use this on 1 or 2 elements, you may just want to change it to the winforms counterpart(s))


--Contact--
Email: michaelreeves808@gmail.com


--Visit Me--
My Portfolio: michaelreeves.us
My Company Website: www.infibit.net