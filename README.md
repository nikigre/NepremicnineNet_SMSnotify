# Nepremicnine.netNotify
Nepremicnine.netNotify is a C# program that checks [nepremicnine.net](https://nepremicnine.net) website for new ads. If it finds any, it sends an SMS to the desired phone number.

In the middle of the summer, I was searching for an apartment in Ljubljana. I was on Nepremicnine.net all the time in search of a good apartment near the centre and my college. But almost whenever I found it, it would be taken by someone else who found it before me. This website has an automatic notifying option, but it sends an email at 1.00 in the morning. Useless... 😔

So one night I got an idea! How about if a write a program that checks the website for any new ads and then notifies me. So I did that. It is a really basic program that checks the website periodically and sends an SMS when a new ad is published.

So to finish this story. I found a really good apartment thanks to this app. 🥳

## How to configure it for your needs
At the top of the program, you will see four variables which you need to set.
```C#
static readonly string NepremecnineNetURL = "URL";
static readonly string SMSsenderAPIkey = "KEY";
static readonly int ThreadSleep = 1000 * 60 * 5;
static readonly string[] PhoneNumbers = new string[] {
    "00386xxxxxxxx"
};
```
### Variable NepremecnineNetURL
Go to [https://www.nepremicnine.net/24ur.html](https://www.nepremicnine.net/24ur.html) and set the parameters of a search for your dream apartment. So for example:
* Country: Slovenia
* Region: LJ-city
* Type of offer: For rent
* Regional subunits: LJ-Bežigrad, LJ-Center, LJ-Šiška
* Price: up to 500 €

This example will generate this link: [https://www.nepremicnine.net/24ur/oglasi-oddaja/ljubljana-mesto/ljubljana-bezigrad,ljubljana-siska,ljubljana-center/stanovanje/cena-do-500-eur-na-mesec/](https://www.nepremicnine.net/24ur/oglasi-oddaja/ljubljana-mesto/ljubljana-bezigrad,ljubljana-siska,ljubljana-center/stanovanje/cena-do-500-eur-na-mesec/)

Then just copy the URL for a website and paste it into NepremecnineNetURL.

### Variable SMSsenderAPIkey
For sending SMS via SMSsenderAPI please contact me (email, contect form on my website) and I will give you a key 😊. More about the API you can read it [here](https://nikigre.si/sl/sms-sender-api/).

When you get the key, just paste it into this variable.

### Variable ThreadSleep
This variable sets the interval of checking the website. I recommend it at 5 minutes. 

`ms * seconds * minutes = 5 min`

### Variable PhoneNumbers
This is an array of Slovenian phone numbers to which you want to send an SMS when the new ad is published.