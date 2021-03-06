//**************************CARRERA-VERSION 2**********************************
//*****************************************************************************
#include <EEPROM.h>
#define SpeedPin 9                                                              //Steuert Basis vom Transistor
#define SensorPin 0                                                             //Aktuell Potentiometer, später Handdynamometer
#define EEPROM_VehicleCalibrationValue 0                                        //Nullpunktdefinition der Geschwindigkeit

word SpeedValue = 0, SignalValue = 0;                                           //Geschwindigkeit des Fahrzeuges in PWM (0-255)
word MaxSpeedValue = 255;                                                       //Max. Geschwindigkeit des Fahrzeuges
byte VehicleCalibrationValue, SpeedFactor = 1;
bool RunFlag = true;

//Serial
String Data_Phrase = "DATA";
String Cal_Phrase = "CAL";
String SFactor_Phrase = "SFACTOR";
String Control_Phrase = "CONTROL";
String VMax_Phrase = "VMAX";
String Start_Phrase ="START";
String End_Phrase = "END";

void setup() {
  pinMode(13, OUTPUT);
  pinMode(SpeedPin, OUTPUT);                                                    //setzen des Ausgangspin für den Transistor
  Serial.begin(9600);
  VehicleCalibrationValue = EEPROM.read(EEPROM_VehicleCalibrationValue);        //Kalibrierungswert aus dem EEPROM auslesen.
  digitalWrite(13, HIGH);
}

void loop() {

  drive();

  if (Serial.available())
  {
      serialInput(Serial.readStringUntil(';'));
      Serial.flush();
  }

}
