// #####################################################################################################################
// ######################################### SERIAL INPUT ##############################################################
// ##################################################################################################################### 
void serialInput(String Parameter_Data)
{
	// Begin connection
	if (Parameter_Data.startsWith(Connection_Phrase))
	{
		Connected = true;
		// Delay for stability
		delay(100);
		// First, send the software version, so the host knows what the controller supports
		String Version = Data_Version_Phrase + Software_Version;
		serialOutput(Version);
		// Clear seriel data
		Serial.flush();
		// Print the connected logo
		lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8);
		delay(100);
		// Send back begin message, so the host knows, it is connected
		serialOutput(Connection_Phrase);
	}
	// End connection
	else if (Parameter_Data.startsWith(Disconnection_Phrase))
	{
		Connected = false;
		digitalWrite(Output_Valve_1, LOW);
		digitalWrite(Output_Valve_2, LOW);
		digitalWrite(Output_Valve_3, LOW);
		// The host needs this message, so it knows, the arduino is really diconnected
		serialOutput(Disconnection_Phrase);
		lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
	}
	// Receive data block (for the working process)
	else if (Parameter_Data.startsWith(Data_Phrase) && Connected)
	{
		digitalWrite(Output_LED, HIGH);
		processData(Parameter_Data);
		digitalWrite(Output_LED, LOW);
	}
	// Receive command (for single operations)
	else if (Parameter_Data.startsWith(Command_Phrase) && Connected)
	{
		digitalWrite(Output_LED, HIGH);
		processCommand(Parameter_Data);
		digitalWrite(Output_LED, LOW);
	}
	// Undefined data
	else
	{
		
	}
}
// #####################################################################################################################
// ######################################### SERIAL OUTPUT #############################################################
// ##################################################################################################################### 
void serialOutput(String Parameter_Data)
{
	Serial.print(Parameter_Data + ";");
}
// ##################################################################################################################### 
// ######################################### SPLIT STRING ##############################################################
// ##################################################################################################################### 
String splitString(String Parameter_String, char Parameter_Separator, int Parameter_Index) 
{ 
	byte Separator_Index = 0; 
	byte String_Index[]  = {0, -1}; 
 	byte Maximum_Index   = Parameter_String.length() - 1; 
 	// Separate the string 
 	for (byte X = 0; X <= Maximum_Index && Separator_Index <= Parameter_Index; X++) 
 	{ 
 		if (Parameter_String.charAt(X) == Parameter_Separator || X == Maximum_Index) 
 		{ 
 			Separator_Index++; 
 			String_Index[0] = String_Index[1] + 1; 
 			String_Index[1] = X == Maximum_Index ? X + 1 : X; 
 		} 
 	} 
 	return Separator_Index > Parameter_Index ? Parameter_String.substring(String_Index[0], String_Index[1]) : ""; 
}
// ##################################################################################################################### 
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 