This repository is created for login to Angelone smartapi and connect to latest Wbsocket version 2.
It works seamlessly as standalone project.
If you clone it and start locally, it may give error 401 Unauthorised access . In that case, delete angelone.txt file in Debug/release folder and Start. It will generate new token.
Copy user.txt file in application/Debug/release folder and fill your ClientID, Password,APIkey, TOTPkey as instructed. 
If you Copy and paste this code as it is into any other C# application, it starts giving AG8001 Invalid Token error at anytime. This is highly frustrating of Angel Broking.
However, standalone project works seamlessly. It can use saved token or generate new one at your will.
