# Water Drop (Pro)
A DELOARTS Research project.

### 1. MAIN CHANGES TO WATER DROP (ADVANCED)
- It is not standalone anymore
- The parameters are now set with a host (via USB)
- It supports valves

### 2. DESCRIPTION
Water Drop (Pro), is a host-based microcontroller project, which allows you to **capture a water drop** with a DSLR camera.
The water drop is created with an electro-magnetic valve. You can use 3 valves max.

On this repository you will not only find the source code for the microcontroller, but also the
software for the host.

On my blog you will find a complete description, to get there, please click the link: --- currently no blog post online ---

If you want to direct download the whole bundle please click this link: http://deloarts.bplaced.net/software/water_drop_pro/wdp.zip

The bundle contains the following items:
- Microcontroller source code
- PC host source code 
- Electrical engineering
  - Breadboard circuit
  - PCB circuic
  - Part list
- Mechanical engineering
  - Design drawings
  - Assembly plan
  - Part list
- Step-by-step description for the whole thing

##### 2.1. MICROCONTROLLER
The necessary microcontroller I used is the ATmega328 on an **Arduino Nano V3.0** board. Therefore I used the Arduino IDE to compile and flash the controller.
You can also use an Ardunio Uno Rev3 board, but I do not recommend it, because you won't be able to use my PCB board.
If you're going to use the Arduino Uno you don't have to make any changes in the source code, just make sure you have selected the correct board in the Ardunio IDE.
You also don't have to change the pin occupancy, when the UNO is replaced with the Nano, and vice versa.

##### 2.2. HOST
The Water Drop (Pro) host software only runs on **Microsoft Windows OS** (any CPU, .NET Framework 4.5)
- Microsoft Windows XP
- Microsoft Windows Vista
- Microsoft Windows 7
- Microsoft Windows 8
- Microsoft Windows 8.1
- Microsoft Windows 10

The source code is written in **C#** and was compiled with Microsoft Visual Studio 2012.

The host software is capable of the following functions:
- Connecting the microcontroller with the PC (via USB)
- Sending necessary data to the controller
- Receiving necessary data from the controller
- Saving valve data to the hard drive
- Controlling a DSLR (via USB, Canon only [currently])
  - Aperture
  - Shutter speed
  - ISO
  - Live-View

A multi-platform host software will come later. This new software will be written in Python and will have the project name "DropShot".

### 3. DISCLAIMER
I am **not** responsible for any damages on the Ardunio Board, your PCB board, the host PC or on the used DSLR-Camera.
I have built and tested everything very precisely and did not damage any of my components.