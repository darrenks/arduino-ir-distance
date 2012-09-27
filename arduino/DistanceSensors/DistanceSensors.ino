/*
 
 */

int ir1 = A0;    // pin for IR distance sensor 1
int ir2 = A1;    // pin for IR distance sensor 2
int ir3 = A2;    // pin for IR distance sensor/potentiometer 3
int ir4 = A3;
int ledPin = 13; // select the pin for the LED
int analog1 = 0; // variable to store the value coming from the first sensor
int analog2 = 0; // variable to store the value coming from the second sensor
int analog3 = 0; // variable to store the value coming from the third sensor
int analog4 = 0;

void setup() {

  pinMode(ledPin, OUTPUT);
  //pinMode(ledPin, INPUT);
  
  Serial.begin(9600);
}

void loop() {
  
  // read the value from the sensors:
  // TODO: uncomment when we have the sensors
  analog1 = analogRead(ir1);
  analog2 = analogRead(ir2);
  analog3 = analogRead(ir3);
  analog4 = analogRead(ir4);
  
  // TODO: convert to distance values
  
  // write values to serial port
  Serial.print("=");
  Serial.print(analog1);
  Serial.print(",");
  Serial.print(analog2);
  Serial.print(",");
  Serial.print(analog3);
  Serial.print(",");
  Serial.print(analog4);
  Serial.println("");
  
  // turn the ledPin on
  //digitalWrite(ledPin, HIGH);  
  // stop the program for <sensorValue> milliseconds:
  //delay(sensorValue);          
  // turn the ledPin off:        
  //digitalWrite(ledPin, LOW);   
  // stop the program for for <sensorValue> milliseconds:
  //delay(sensorValue);                  
}
