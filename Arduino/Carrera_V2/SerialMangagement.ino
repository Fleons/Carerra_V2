// #####################################################################################################################
// ######################################### SERIAL INPUT#############################################################
// #####################################################################################################################

void serialInput(string Parameter_Data)
{
      if(Parameter_Data.startswith(Data_Phrase))
      {
        serialOutput(Data_Phrase+ "|" + SignalValue);
      }
      else if (Parameter_Data.startswith(Cal_Phrase))
      {
        VehicleCalibrationValue= splitString(Cal_Phrase, ,"|", 1);
      }
      else if (Parameter_Data.startswith(SFactor_Phrase))
      {
        SpeedFactor= splitString(SFactor_Phrase, ,"|", 1);
      }
      else if (Parameter_Data.startswith(Control_Phrase))
      {
        StringTemp = splitString(Control_Phrase, ,"|", 1);
        if(StringTemp == Start_Phrase)
        {
          RunFlag = true;
        }
        else if (StringTemp == End_Phrase)
        {
          RunFlag = false;
          analogWrite(SpeedPin,0);
        }

      }
      else if (Parameter_Data.startswith(VMax_Phrase))
      {
        MaxSpeedValue = splitString(VMax_Phrase, ,"|", 1);
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
