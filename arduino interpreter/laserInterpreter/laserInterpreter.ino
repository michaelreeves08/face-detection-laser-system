#include<Servo.h>

Servo serX;
Servo serY;

String tempModify;

void setup() {

  serX.attach(11);
  serY.attach(10);
  Serial.begin(9600);
  Serial.setTimeout(10);
}

void loop() {
  //lol
}

void serialEvent() {
tempModify = Serial.readString();

serX.write(parseDataX(tempModify));
serY.write(parseDataY(tempModify));

}

int parseDataX(String data){
  data.remove(data.indexOf(":"));
  data.remove(data.indexOf("X"), 1);

  return data.toInt();
}

int parseDataY(String data){
  data.remove(0,data.indexOf(":") + 1);
  data.remove(data.indexOf("Y"), 1);

  return data.toInt();
  
}

