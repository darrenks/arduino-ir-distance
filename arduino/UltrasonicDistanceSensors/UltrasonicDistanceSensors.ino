/*
 * Define the pins you want to use as trigger and echo.
 */

#define ECHOPIN1 2              // Pin to receive echo pulse
#define TRIGPIN1 3              // Pin to send trigger pulse
#define ECHOPIN2 5              // Pin to receive echo pulse
#define TRIGPIN2 6              // Pin to send trigger pulse

#define filterWidth 5

int triggerPullDown = 2;        // in microseconds
int pingWidth = 10;             // in microseconds
int maxDist = 80;               // in cm
int timeout = maxDist * 2 * 29; // in microseconds
// timeout derivation from user JamesHappy on arduino.cc forum: http://arduino.cc/forum/index.php?topic=55119.0

float d1[filterWidth];
float d2[filterWidth];
int dindex = 0;

void setup()
{
  Serial.begin(9600);
  pinMode(ECHOPIN1, INPUT);
  pinMode(TRIGPIN1, OUTPUT);
  pinMode(ECHOPIN2, INPUT);
  pinMode(TRIGPIN2, OUTPUT);
  
  for(int i = 0; i < filterWidth; i++)
  {
    d1[i] = 0;
    d2[i] = 0;
  }
}

void loop()
{
  // Send a ping on 1
  digitalWrite(TRIGPIN1, LOW);
  delayMicroseconds(triggerPullDown);
  digitalWrite(TRIGPIN1, HIGH);
  delayMicroseconds(pingWidth);
  digitalWrite(TRIGPIN1, LOW);
  
  // Listen for echo and compute distance
  float distance1 = pulseIn(ECHOPIN1, HIGH, timeout);
  distance1= distance1/58; // convert to centimeters
  if(distance1 <= 0) distance1 = maxDist;
  
  // Send a ping on 2
  digitalWrite(TRIGPIN2, LOW);
  delayMicroseconds(triggerPullDown);
  digitalWrite(TRIGPIN2, HIGH);
  delayMicroseconds(pingWidth);
  digitalWrite(TRIGPIN2, LOW);
  
  // Listen for echo and compute distance
  float distance2 = pulseIn(ECHOPIN2, HIGH, timeout);
  distance2= distance2/58; // convert to centimeters
  if(distance2 <= 0) distance2 = maxDist;
  
  d1[dindex] = distance1;
  d2[dindex] = distance2;
  dindex++;
  if(dindex >= filterWidth) dindex = 0;
  
  float avg1 = 0, avg2 = 0, count1 = 0, count2 = 0;
  for(int i = 0; i < filterWidth; i++)
  {
    if(d1[i] < maxDist) { avg1 += d1[i]; count1++; }
    if(d2[i] < maxDist) { avg2 += d2[i]; count2++; }
  }
  if(count1 > 0) avg1 /= (float)count1; else avg1 = maxDist;
  if(count2 > 0) avg2 /= (float)count2; else avg2 = maxDist;
  
  // Write distance readings to serial
  Serial.print("=");
  Serial.print(avg1);
  Serial.print(",");
  Serial.print(avg2);
  Serial.println("");
  
  // Wait for residual echos to dissipate
  delay(50);
}

