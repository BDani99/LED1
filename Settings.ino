#include <FastLED.h>
#define LED_PIN     7   // Arduino 7. digit pin
#define NUM_LEDS    21  // LED-ek száma
CRGB leds[NUM_LEDS];  



void setup() {
Serial.begin(9600);   // Soros kapcsolat kezdete
FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);  // FastLED-ek hozzáadása
}

// Deklarációk
String beolvas = "";  
String operation="";
String lednum="";
int j = 0;
String aktlednum = "";

void loop() {
 
 beolvas =Serial.readString(); // A GUI-ból kapott érték beolvasása  
  if(beolvas != ""){ 
    //Jelzőbit, milyen műveletet akarunk elvégezni. 
    //(A->item berakás, B->item kivétel, C->Ellenőrzés, D->Leltározás)
    operation = beolvas[0]; 
    lednum = beolvas.substring(1); //Megvilágitandó LED(ek) sorszáma.
    j = 0;
  //Betesz
    if (operation=="A"){
      // A LED-ek megvilágítása Zöldel       
      leds[lednum.toInt()] = CRGB(255, 0, 0);
      FastLED.show();
      delay(2500); //LED megvilágitás ideje.
      //LED kikapcsolása
      leds[lednum.toInt()] = CRGB(0, 0, 0);
      FastLED.show();
      delay(500);
    }
    //Kivesz
    else if(operation=="B"){
      // A LED-ek megvilágítása Lilával       
      leds[lednum.toInt()] = CRGB(0, 255, 255);
      FastLED.show();
      delay(2500);
      //LED kikapcsolása
      leds[lednum.toInt()] = CRGB(0, 0, 0);
      FastLED.show();
      delay(500);
    }
    //Ellenőrzés, összes led bekapcsolása
    else if(operation=="C"){
      for (int i = 0; i <= NUM_LEDS; i++) {
        leds[i] = CRGB ( 4, 7, 4);
        FastLED.show();
      }
      delay(2500);
      //Ledek kikapcsolása
      FastLED.clear();
      FastLED.show();
    }
    //Leltarozás, ledek bekapcsolása ott ahol van item.
    //Input: String lednum, ami számokat tartalmaz ','-vel elválasztva. 
    //(Példa: 2,3,6,11,20,)
    else if(operation=="D"){
      while(j < lednum.length()){
        aktlednum = "";
        while(lednum[j] != ','){
          aktlednum += lednum[j]; //Több számjegyű int-hez
          j++;
        }
      leds[aktlednum.toInt()] = CRGB ( 128, 255, 0);
      FastLED.show();
      j++;
      }
      delay(2500);
      j = 0;
      
      //Ledek lekapcsolása
      FastLED.clear();
      FastLED.show();
     }
    
  }//nagy if
}//loop
  
 
