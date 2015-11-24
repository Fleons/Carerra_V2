// ##################################################################################################################### 
// ######################################### STARTUP ###################################################################
// ##################################################################################################################### 
void startup()
{
	// Setup display
	lcd.initialize();
	lcd.rotateDisplay180();
	lcd.setBrightness(250);
	// Define input pins
	pinMode(Input_Button_Valve_1, INPUT);
	pinMode(Input_Button_Valve_2, INPUT);
	pinMode(Input_Button_Valve_3, INPUT);
	// Define output pins
	pinMode(Output_Valve_1, OUTPUT);
	pinMode(Output_Valve_2, OUTPUT);
	pinMode(Output_Valve_3, OUTPUT);
	pinMode(Output_Focus,   OUTPUT);
	pinMode(Output_Camera,  OUTPUT);
	pinMode(Output_LED,     OUTPUT);
	// Set pins
	digitalWrite(Output_Valve_1, LOW);
	digitalWrite(Output_Valve_2, LOW);
	digitalWrite(Output_Valve_3, LOW);
	digitalWrite(Output_Focus,   LOW);
	digitalWrite(Output_Camera,  LOW);
	digitalWrite(Output_LED,     LOW);
	// Show disconnected logo
	lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
}
// #####################################################################################################################  
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 