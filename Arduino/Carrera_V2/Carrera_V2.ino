//**************************CARRERA-VERSION 2**********************************
//*****************************************************************************
#include <EEPROM.h>
#define SpeedPin 9                                                              //Steuert Basis vom Transistor
#define SensorPin 0                                                             //Aktuell Potentiometer, später Handdynamometer

word SpeedValue;                                                                //Geschwindigkeit des Fahrzeuges in PWM (0-255)
word MaxSpeedValue = 255;                                                       //Max. Geschwindigkeit des Fahrzeuges
word PWM;
byte VehicleCalibrationValue, SpeedFactor = 1;

//Serial
String Data_Phrase = "DATA";
String Cal_Phrase = "CAL";
String SFactor_Phrase = "SFACTOR";
String Control_Phrase = "CONTROL";
String VMax_Phrase = "VMAX";



void setup() {
  pinMode(SpeedPin, OUTPUT);                                                    //setzen des Ausgangspin für den Transistor
  Serial.begin(9600);
  VehicleCalibrationValue = EEPROM.read(EEPROM_VehicleCalibrationValue);        //Kalibrierungswert aus dem EEPROM auslesen.


}

void loop() {
  if (Serial.available())
  {
      serialIntput(readStringUntil(";"));                                       //Funktion in SerialManagement.ino

  }

}
