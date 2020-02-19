 
int trigPin = 2;    //Trig 핀 할당
int echoPin = 4;    //Echo 핀 할당

  
void setup()  
{
  pinMode(trigPin, OUTPUT);    //Trig 핀 output으로 세팅
  pinMode(echoPin, INPUT);    //Echo 핀 input으로 세팅
  Serial.begin(19200);    
}  
  
void loop()  
{   
    int duration, distance;    //기본 변수 선언
 
    //Trig 핀으로 10us의 pulse 발생
    digitalWrite(trigPin, LOW);        //Trig 핀 Low
    delayMicroseconds(2);            //2us 유지
    digitalWrite(trigPin, HIGH);    //Trig 핀 High
    delayMicroseconds(10);            //10us 유지
    digitalWrite(trigPin, LOW);        //Trig 핀 Low

    //Echo 핀으로 들어오는 펄스의 시간 측정
    duration = pulseIn(echoPin, HIGH);  //pulseIn함수가 호출되고 펄스가 입력될 때까지의 시간. us단위로 값을 리턴.
 
    distance = duration / 29 / 2;        //센치미터로 환산

    String a = String(distance);
    Serial.println(a);
 
    delay(10);

     
}  
