// #####################################################################################################################  
// ######################################### PROCESS DATA (WORKING PROCESS) ############################################
// ##################################################################################################################### 
void processData(String Parameter_Data)
{
	/*
	SERIAL PROTOCOL ORDER (index list)

		 0: Keyword "DATA" (not used here anymore)
		 1: Delay for camera
		 2: Amount of repeats
		 3: Pause between repeats
		 4: Focus enabled
		 5: Mirror lockup enabled
		 6: V1D1 enabled
		 7: V1D1S
		 8: V1D1D
		 9: V1D2 enabled
		10: V1D2S
		11: V1D2D
		12: V1D3 enabled
		13: V1D3S
		14: V1D3D
		15: V1D4 enabled
		16: V1D4S
		17: V1D4D
		18: V2D1 enabled
		19: V2D1S
		20: V2D1D
		21: V2D2 enabled
		22: V2D2S
		23: V2D2D
		24: V2D3 enabled
		25: V2D3S
		26: V2D3D
		27: V2D4 enabled
		28: V2D4S
		29: V2D4D
		30: V3D1 enabled
		31: V3D1S
		32: V3D1D
		33: V3D2 enabled
		34: V3D2S
		35: V3D2D
		36: V3D3 enabled
		37: V3D3S
		38: V3D3D
		39: V3D4 enabled
		40: V3D4S
		41: V3D4D
	*/
	// Get data from serial block
	Trigger_Delay  = splitString(Parameter_Data, '|', 1).toInt();
	Repeats_Amount = splitString(Parameter_Data, '|', 2).toInt();
	Repeats_Pause  = splitString(Parameter_Data, '|', 3).toInt() * 1000;
	Settings_Focus = splitString(Parameter_Data, '|', 4).toInt();
	Settings_Mirror_Lockup = splitString(Parameter_Data, '|', 5).toInt();

	byte v1 = 6, v2 = 18, v3 = 30;
	for (byte x = 0; x < 4; x++)
	{
		Valve_1[x] 			 = splitString(Parameter_Data, '|', v1).toInt();
		Valve_1_Start[x] 	 = splitString(Parameter_Data, '|', v1 + 1).toInt();
		Valve_1_Durration[x] = splitString(Parameter_Data, '|', v1 + 2).toInt();

		Valve_2[x] 			 = splitString(Parameter_Data, '|', v2).toInt();
		Valve_2_Start[x] 	 = splitString(Parameter_Data, '|', v2 + 1).toInt();
		Valve_2_Durration[x] = splitString(Parameter_Data, '|', v2 + 2).toInt();

		Valve_3[x] 			 = splitString(Parameter_Data, '|', v3).toInt();
		Valve_3_Start[x] 	 = splitString(Parameter_Data, '|', v3 + 1).toInt();
		Valve_3_Durration[x] = splitString(Parameter_Data, '|', v3 + 2).toInt();

		v1 += 3;
		v2 += 3;
		v3 += 3;
	}

	// Working process will be enabled now
	serialOutput(Complete_Start_Phrase);
	// Print the working logo
	lcd.drawBitmap(Bitmap_Working, 0, 0, 16, 8);

	// Working process starts here
	outputManagement();
	
	// Working process has been ended
	serialOutput(Complete_End_Phrase);
	// Print the connected logo
	lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8);
}
// #####################################################################################################################  
// ######################################### PROCESS DATA (SINGLE OPERATIONS) ##########################################
// ##################################################################################################################### 
void processCommand(String Parameter_Command)
{
	// Toggle valve
	if (splitString(Parameter_Command, '|', 1) == Command_Toggle_Valve)
	{
		switch (splitString(Parameter_Command, '|', 2).toInt())
		{
			// Close valve 1
			case 10:
				digitalWrite(Output_Valve_1, LOW);
				Valve_Open[0] = false;
				Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
				break;
			// Open valve 1
			case 11:
				digitalWrite(Output_Valve_1, HIGH);
				Valve_Open[0] = true;
				lcd.drawBitmap(Bitmap_Drop_One, 0, 0, 16, 8);
				break;
			// Close valve 2
			case 20:
				digitalWrite(Output_Valve_2, LOW);
				Valve_Open[1] = false;
				Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
				break;
			// Open valve 2
			case 21:
				digitalWrite(Output_Valve_2, HIGH);
				Valve_Open[1] = true;
				lcd.drawBitmap(Bitmap_Drop_Two, 0, 0, 16, 8);
				break;
			// Close valve 3
			case 30:
				digitalWrite(Output_Valve_3, LOW);
				Valve_Open[2] = false;
				Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
				break;
			// Open valve 3
			case 31:
				digitalWrite(Output_Valve_3, HIGH);
				Valve_Open[3] = true;
				lcd.drawBitmap(Bitmap_Drop_Three, 0, 0, 16, 8);
				break;
		}
	}
}
// ##################################################################################################################### 
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 