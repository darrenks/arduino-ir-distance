/*
 * Define the pins you want to use as trigger and echo.
 */

#define ECHOPIN1 2              // Pin to receive echo pulse
#define TRIGPIN1 3              // Pin to send trigger pulse
#define ECHOPIN2 5              // Pin to receive echo pulse
#define TRIGPIN2 6              // Pin to send trigger pulse

int triggerPullDown = 2;        // in microseconds
int pingWidth = 10;             // in microseconds
int maxDist = 80;               // in cm
int timeout = maxDist * 2 * 29; // in microseconds
// timeout derivation from user JamesHappy on arduino.cc forum: http://arduino.cc/forum/index.php?topic=55119.0

/*
 * setup function
 * Initialize the serial line (D0 & D1) at 115200.
 * Then set the pin defined to receive echo in INPUT 
 * and the pin to trigger to OUTPUT.
 */
 
void setup()
{
  Serial.begin(115200);
  pinMode(ECHOPIN1, INPUT);
  pinMode(TRIGPIN1, OUTPUT);
  pinMode(ECHOPIN2, INPUT);
  pinMode(TRIGPIN2, OUTPUT);
}

/*
 * loop function.
 * 
 */
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
  
  // Write distance readings to serial
  Serial.print("=");
  Serial.print(distance1);
  Serial.print(",");
  Serial.print(distance2);
  Serial.println("");
  
  // Wait for residual echos to dissipate
  delay(150);
}

