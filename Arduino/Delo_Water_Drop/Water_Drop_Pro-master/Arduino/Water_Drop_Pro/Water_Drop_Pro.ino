// #####################################################################################################################
// ######################################### WATER DROP PRO ############################################################
// #####################################################################################################################
/*
    DELOARTS RESEARCH INC.
    WATER DROP PRO 1.2.0
	08.08.2015
	
	DESCRIPTION
		Water Drop (Pro) is a host-based microcontroller project, which allows you to capture a water
		drop by using electro-magnetic valves. This code runs on Arduino devices with a host (PC, Raspberry, etc.).

	MAIN CHANGES TO WATER DROP ADVANCED
		- It is not standalone anymore
		- The parameters are now set with a host (via USB)
		- It supports valves

	MICROCONTROLLER
		- Arduino UNO Rev3
		- Arduino Nano v3

		There is no need to change the pin occupancy, when the UNO is replaced with the Nano, and vice versa.
		Just be careful, that you change the board in the settings!

	PROTOCOL
		There are multiple keywords for the protocol

		- BEGIN:	Start connection with Arduino/PC 	(A <-> PC)
		- END:		End the connection 					(A <-> PC)
		- DATA: 	A data block is following 			(A <-> PC)
		- COMMAND:  A command block is following        (A <-  PC)
		- COMPLETE:	The Arduino received all data 		(A  -> PC)
*/
// Define code settibgs
//#define Test_Mode
// Get modules
#include <avr/pgmspace.h>
#include <avr/wdt.h> 
#include <OLED_I2C_128x64_Monochrome.h>
#include <Wire.h>
// #####################################################################################################################  
// ######################################### VARIABLES #################################################################
// #####################################################################################################################
// Input
#define Input_Sensor			A0

#define Input_Button_Valve_1	3
#define Input_Button_Valve_2	4
#define Input_Button_Valve_3	5
// Output
#define Output_Vcc_IR_LED		6
#define Output_Vcc_Sensor		7

#define Output_Valve_1			8
#define Output_Valve_2			9
#define Output_Valve_3			10

#define Output_Focus    		11
#define Output_Camera   		12
#define Output_LED				13
// Basic
String Software_Version = "1.2.0";
unsigned long Timer_State = 0;
boolean LED_State = false;
// Serial
boolean Connected = false;
String Connection_Phrase = "BEGIN";
String Disconnection_Phrase = "END";
String Data_Phrase = "DATA";
String Data_Version_Phrase = "DATA|VERSION|";
String Complete_Start_Phrase = "COMPLETE|START";
String Complete_End_Phrase = "COMPLETE|END";
String Command_Phrase = "COMMAND";
String Command_Toggle_Valve = "TOGGLE";
// Valves
boolean Valve_Open[3] = {0};
// Working
unsigned long Trigger_Delay = 0;
int Repeats_Amount = 0;
int Repeats_Pause = 0;
boolean Settings_Focus = 0;
boolean Settings_Mirror_Lockup = 0;
boolean Valve_1[4] = {0};
unsigned long Valve_1_Start[4] = {0};
unsigned long Valve_1_Durration[4] = {0};
boolean Valve_2[4] = {0};
unsigned long Valve_2_Start[4] = {0};
unsigned long Valve_2_Durration[4] = {0};
boolean Valve_3[4] = {0};
unsigned long Valve_3_Start[4] = {0};
unsigned long Valve_3_Durration[4] = {0};

// #####################################################################################################################  
// ######################################### SETUP #####################################################################
// ##################################################################################################################### 
void setup()
{
	// Start serial com
	Serial.begin(9600);
	// Begin with startup
	startup();
}
// #####################################################################################################################   
// ######################################### LOOP ######################################################################
// ##################################################################################################################### 
void loop()
{
	// Get serial data
	if (Serial.available() > 0)
	{
		serialInput(Serial.readStringUntil(';'));
		// Clear buffer
		Serial.flush();
	}
	// Prevent timer overflow (about 46 days)
	else if (millis() > 4000000000)
	{
		serialOutput(Disconnection_Phrase);
		// Reset arduino
		wdt_enable(WDTO_250MS); 
	}
	// Switch LED depending on connection state
	else if (millis() > Timer_State)
	{
		Connected ? Timer_State = millis() + 250 : Timer_State = millis() + 750;
		LED_State = !LED_State;
		digitalWrite(Output_LED, LED_State);
	}
	// Get input
	else
	{
		inputManagement();
	}
}
// ##################################################################################################################### 
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 