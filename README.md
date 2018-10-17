# ISO-8583 .Net Lib

#### ISO8583 Message Packer and Unpakcer with ISOClient for communication with iso server 



A lightweight ISO8583 (is an international standard for financial transaction card originated interchange messaging - [wikipedia][iso8583-Wiki] ) library for DotNet based on builder pattern and provide very simple use as you will see later.



### Usage

Library is available in [Nuget][nuget]


## ISOMessage
#### Create and pack an ISO message
To create an ISO message you must use ISOMessageBuilder which produce iso message for you:

```csharp

ISOMessage isoMessage = ISOMessageBuilder.Packer(VERSION.V1987)
                .NetworkManagement()
                .MTI(MESSAGE_FUNCTION.Request, MESSAGE_ORIGIN.Acquirer)
                .ProcessCode("930000")
                .SetField(FIELDS.F11_STAN, "385629")
                .SetField(FIELDS.F22_EntryMode, "1234")
                .SetField(FIELDS.F24_NII_FunctionCode, "4321")
                .SetField(FIELDS.F25_POS_ConditionCode, "12")
                .SetField(FIELDS.F41_CA_TerminalID, "12345678")
                .SetField(FIELDS.F42_CA_ID, "123456789876543")
                .GenerateMac(data => {
			byte[] mac = MacCalculator(data);
			return mac;
		})
                .SetHeader("1234567890")
                .Build();
				
                
```
with `ISOMessageBuilder.Packer(VERSION.V1987)` you can build iso message as you can see above. the 'Packer' method return 8 method for 8 iso message class (authorization, financial, networkManagment, ...) based on ISO8583 after that you can set message function and message origin by `MTI` method.
`MTI` method accept string and enums as parameter, and I think enums are much clear and readable.
As you know an iso message need a 'Processing Code' and you can set it's value by `ProcessCode` method, and then we can start setting required fields by `SetField` method and accept String and enums as field number parameter.
After all, you must call build method to generate iso message object.
#### Unpack a buffer and parse it to ISO message
For unpacking a buffer received from server you need to use `ISOMessageBuilder.Unpacker()`:

```csharp
ISOMessage isoMessage = ISOMessageBuilder.Unpacker()
                                    .SetMessage(SAMPLE_HEADER + SAMPLE_MSG)
                                    .Build();
```
#### Working with ISOMessage object
ISOMessage object has multiple method provide fields, message body, header and ...
for security reason they will return byte array exept `.ToString` and `.GetStringField` method, because Strings stay alive in memory until a garbage collector will come to collect that. but you can clear byte or char arrays after use and calling garbage collector is not important anymore.
If you use Strings, taking memory dumps will be dangerous.
```csharp
    byte[] body = isoMessage.GetBody();
```
```csharp
    byte[] trace = isoMessage.GetField(11);
```
```csharp
    string trace = isoMessage.GetStringField(FIELDS.F11_STAN);
```
## ISOClient
#### Sending message to iso server
Sending message to iso server and received response from server can be done with ISOClient in many ways:
```csharp
IISOClient client = ISOClientBuilder.CreateSocket(HOST, PORT)
                .Build();

        ISOMessage respIso = client.SendMessageSync(reqIso);
        client.Disconnect();
```



License
-------
Copyright 2018 Mohsen Beiranvand

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

   [iso8583-Wiki]: <https://en.wikipedia.org/wiki/ISO_8583/>
   [nuget]: <https://www.nuget.org/packages/imohsenb.iso8583/>
   [mit]: <https://opensource.org/licenses/MIT/>

