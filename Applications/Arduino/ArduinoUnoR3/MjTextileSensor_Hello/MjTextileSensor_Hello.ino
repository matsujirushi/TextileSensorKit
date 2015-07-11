#include <Wire.h>

void setup() {
  Serial.begin(9600);
  Wire.begin();

  // GetVersion
  Wire.beginTransmission(0x42);
  Wire.write(0x00);
  Wire.endTransmission();

  Wire.requestFrom(0x42, 3);
  Wire.read();
  byte minorVersion = Wire.read();
  byte majorVersion = Wire.read();

  Serial.print("Firmware version is ");
  Serial.print(majorVersion);
  Serial.print(".");
  Serial.print(minorVersion);
  Serial.println(".");
}

void loop() {

  // GetSensorValues
  Wire.requestFrom(0x42, 11);
  Wire.read();
  for (int i = 0; i < 10; i++)
  {
    byte value = Wire.read();
    Serial.print(value);
    Serial.print(" ");
  }
  Serial.println();
  
  delay(1000);
}
