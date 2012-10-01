/*
 
 */
 
 #define filterWidth 8

int ir1 = A0;    // pin for IR distance sensor 1
int ir2 = A1;    // pin for IR distance sensor 2
int ir3 = A2;    // pin for IR distance sensor/potentiometer 3
int ir4 = A3;
int ledPin = 13; // select the pin for the LED

float a1[filterWidth];
float a2[filterWidth];
float a3[filterWidth];
float a4[filterWidth];
int aindex = 0;

void setup() {

  pinMode(ledPin, OUTPUT);
  //pinMode(ledPin, INPUT);
  
  for(int i = 0; i < filterWidth; i++)
  {
    a1[i] = 0;
    a2[i] = 0;
    //a3[i] = 0;
    //a4[i] = 0;
  }
  
  Serial.begin(9600);
}

float getDistance(float analog)
{
  float volts = analog*0.0048828125f;   // value from sensor * (5/1024) - if running 3.3.volts then change 5 to 3.3
  float distance = 27.0f*(float)pow(volts, -1.10f);
  return distance;
}

void loop() {
  
  // read the value from the sensors:
  int analog1 = analogRead(ir1);
  int analog2 = analogRead(ir2);
  //int analog3 = analogRead(ir3);
  //int analog4 = analogRead(ir4);
  
  float distance1 = getDistance(analog1);
  float distance2 = getDistance(analog2);
  
  a1[aindex] = distance1;
  a2[aindex] = distance2;
  //a3[aindex] = analog3;
  //a4[aindex] = distance;
  aindex++;
  if(aindex >= filterWidth) aindex = 0;
  
  float avg1 = 0, avg2 = 0;
  for(int i = 0; i < filterWidth; i++)
  {
    avg1 += a1[i];
    avg2 += a2[i];
    //avg3 += a3[i];
    //avg4 += a4[i];
  }
  avg1 /= (float)filterWidth;
  avg2 /= (float)filterWidth;
  //avg3 /= (float)filterWidth;
  //avg4 /= (float)filterWidth;
  
  // TODO: convert to distance values
  
  // write values to serial port
  Serial.print("=");
  Serial.print(avg1);
  Serial.print(",");
  Serial.print(avg2);
  //Serial.print(",");
  //Serial.print(avg3);
  //Serial.print(",");
  //Serial.print(avg4);
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
