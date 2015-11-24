void drive() {
  SpeedValue = analogRead(SensorPin);                                           //Wert des Sensors
  SignalValue = SpeedValue;
    if (RunFlag) {                                                              //Sonst "NOTAUS"

        //Sensorwert durch 4, da PWM nur bis 255 und Sensor bis 1023
        //SpeedValue hat nun einen Wert im Bereich von 0 bis 255 - dem Kalibrierungswertes
        SpeedValue = map(SpeedValue/4,0,255,0,255-VehicleCalibrationValue);     //Geschwindikeit ab kalibriertem Nullpunkt                    //map(value, fromLow, fromHigh, toLow, toHigh)
        if (SpeedValue != 0){                                                   // Keine Bremsung

        //SpeedValue wird jetzt mit einem definierten Faktor multipliziert.
        //Dadurch kann auch ein Kind den Looping fahren und größere Geschwindigkeiten erreichen.
        //Zuletzt wird der Wert bei einem max. von 255 (max. PWM) abgeschnitten.
        // --> SpeedValue max. Wert somit 255
        SpeedValue = constrain((SpeedFactor * SpeedValue) + VehicleCalibrationValue,0,255);

        SpeedValue = map(SpeedValue,0,255,0,MaxSpeedValue);                     //Max. Geschwindigkeit begrenzen
      }
      else {                                                                    //Bremsung
        SpeedValue = 0;
      }
      analogWrite(SpeedPin, SpeedValue);                                        //Setzen der Geschwindikeit des Fahrzeugs
    }
    delay(50);
}
