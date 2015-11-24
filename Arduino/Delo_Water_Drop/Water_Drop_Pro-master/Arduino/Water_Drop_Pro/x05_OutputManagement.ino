// #####################################################################################################################
// ######################################### OUTPUT MANGEMENT ##########################################################
// ##################################################################################################################### 
void outputManagement()
{
	boolean Const_Valve_1[4] 	   = {0};
	int Const_Valve_1_Start[4] 	   = {0};
	int Const_Valve_1_Durration[4] = {0};
	boolean Const_Valve_2[4] 	   = {0};
	int Const_Valve_2_Start[4] 	   = {0};
	int Const_Valve_2_Durration[4] = {0};
	boolean Const_Valve_3[4] 	   = {0};
	int Const_Valve_3_Start[4] 	   = {0};
	int Const_Valve_3_Durration[4] = {0};
	int Const_Trigger_Delay = Trigger_Delay;

	for (byte x = 0; x < 4; x++)
	{
		Const_Valve_1[x] 		   = Valve_1[x];
		Const_Valve_1_Start[x] 	   = Valve_1_Start[x];
		Const_Valve_1_Durration[x] = Valve_1_Durration[x];
		Const_Valve_2[x] 		   = Valve_2[x];
		Const_Valve_2_Start[x] 	   = Valve_2_Start[x];
		Const_Valve_2_Durration[x] = Valve_2_Durration[x];
		Const_Valve_3[x] 		   = Valve_3[x];
		Const_Valve_3_Start[x] 	   = Valve_3_Start[x];
		Const_Valve_3_Durration[x] = Valve_3_Durration[x];
	}

	// Run everything n-times
	for (byte n = 0; n < Repeats_Amount; n++)
	{
		/* If the autofocus is enabled, it has to be hold the whole time.
		   The delay is for the focus to find its point.
		   I do not recommend to use the auto-focus
		*/
		if (Settings_Focus)
		{
			digitalWrite(Output_Focus, HIGH);
			delay(1000);
		}
		/* If the mirror lockup is enabled, the camera is triggered, to lock it.
		   The camera needs about 500ms of HIGH state to recognize the signal in every state.
		   I use 2000ms, so the shock of the shutter can subside.
		   I really recommend to use the mirror lockup!
		*/
		if (Settings_Mirror_Lockup)
		{
			digitalWrite(Output_Camera, HIGH);
			delay(500);
			digitalWrite(Output_Camera, LOW);
			delay(2500);
		}

		for (byte x = 0; x < 4; x++)
		{
			/* Set valves.
		   	   This is neccessarry, because the valves will be set durring the process.
		   	   If the process shall be repeated, they have to be put in the original state.
			*/
			Valve_1[x] = Const_Valve_1[x];
			Valve_2[x] = Const_Valve_2[x];
			Valve_3[x] = Const_Valve_3[x];
			/* Set times.
			   From now on, this current point is the zero-level for the counter (millis).
			   Every time spawn in the host software, except the durration, is set from this point.
			*/
			Valve_1_Start[x] = Const_Valve_1_Start[x] + millis();
			Valve_1_Durration[x] = Const_Valve_1_Durration[x] + Valve_1_Start[x];
			Valve_2_Start[x] = Const_Valve_2_Start[x] + millis();
			Valve_2_Durration[x] = Const_Valve_2_Durration[x] + Valve_2_Start[x];
			Valve_3_Start[x] = Const_Valve_3_Start[x] + millis();
			Valve_3_Durration[x] = Const_Valve_3_Durration[x] + Valve_3_Start[x];
		}

		// Finally set the delay for the camera
		Trigger_Delay = Const_Trigger_Delay + millis();

		// Start process
		while (true)
		{
			// Trigger
			if (millis() >= Trigger_Delay)
			{
				digitalWrite(Output_Camera, HIGH);
				delay(500);
				// Disable all camera & valve outputs
				digitalWrite(Output_Camera, LOW);
				digitalWrite(Output_Focus, LOW);
				for (byte x = 0; x < 4; x++)
					digitalWrite(Output_Valve_1, LOW);
				break;
			}

			for (byte x = 0; x < 4; x++)
			{
				// Valve 1
				if (Valve_1[x] && millis() > Valve_1_Start[x])
				{
					digitalWrite(Output_Valve_1, HIGH);
				}
				if (Valve_1[x] && millis() > Valve_1_Durration[x])
				{
					digitalWrite(Output_Valve_1, LOW);
					Valve_1[x] = false;
				}
				// Valve 2
				if (Valve_2[x] && millis() > Valve_2_Start[x])
				{
					digitalWrite(Output_Valve_2, HIGH);
				}
				if (Valve_2[x] && millis() > Valve_2_Durration[x])
				{
					digitalWrite(Output_Valve_2, LOW);
					Valve_2[x] = false;
				}
				// Valve 3
				if (Valve_3[x] && millis() > Valve_3_Start[x])
				{
					digitalWrite(Output_Valve_3, HIGH);
				}
				if (Valve_3[x] && millis() > Valve_3_Durration[x])
				{
					digitalWrite(Output_Valve_3, LOW);
					Valve_3[x] = false;
				}
			}
		}
		// Only set repeats pause, if there are more than one rounds, don't use pause in the last round
		if (Repeats_Amount > 1 && n + 1 < Repeats_Amount)
			delay(Repeats_Pause);
	}
	// Show the working done bitmap (and delay a bit, so it can be seen)
	lcd.drawBitmap(Bitmap_Working_Done, 0, 0, 16, 8);
	delay(1000);
}
// #####################################################################################################################   
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 