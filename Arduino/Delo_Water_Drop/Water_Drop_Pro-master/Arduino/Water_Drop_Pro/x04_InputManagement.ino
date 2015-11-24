// ##################################################################################################################### 
// ######################################### INPUT MANGEMENT ###########################################################
// ##################################################################################################################### 
void inputManagement()
{
	// Let valve 1 drop a small drop
	if (digitalRead(Input_Button_Valve_1))
	{
		lcd.drawBitmap(Bitmap_Drop_One, 0, 0, 16, 8);
		digitalWrite(Output_Valve_1, HIGH);
		delay(250);
		digitalWrite(Output_Valve_1, LOW);
		delay(1000);
		Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
	}
	// Let valve 2 drop a small drop
	else if (digitalRead(Input_Button_Valve_2))
	{
		lcd.drawBitmap(Bitmap_Drop_Two, 0, 0, 16, 8);
		digitalWrite(Output_Valve_2, HIGH);
		delay(250);
		digitalWrite(Output_Valve_2, LOW);
		delay(1000);
		Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
	}
	// Let valve 2 drop a small drop
	else if (digitalRead(Input_Button_Valve_3))
	{
		lcd.drawBitmap(Bitmap_Drop_Three, 0, 0, 16, 8);
		digitalWrite(Output_Valve_3, HIGH);
		delay(250);
		digitalWrite(Output_Valve_3, LOW);
		delay(1000);
		Connected ? lcd.drawBitmap(Bitmap_Connected, 0, 0, 16, 8) : lcd.drawBitmap(Bitmap_Disconnected, 0, 0, 16, 8);
	}
}
// #####################################################################################################################
// ######################################### END OF CODE ###############################################################
// ##################################################################################################################### 