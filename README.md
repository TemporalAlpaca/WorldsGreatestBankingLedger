# WorldsGreatestBankingLedger

# Caleb Kauffman - May 2019

This is a sample application created to demonstrate the basic funtionality of a banking ledger.

Included is a console and web application, both written in C Sharp. The web app uses dotnet core 2.1.

Both applications can be run simultaneously and draw from a JSON repository for persistent storage. The applications contain the basic logic of a bank ledger and have basic error handling. The console application uses model and repository classes contained in the web application which allows them to share the same functionality when saving or retrieving data/classes without duplicating code.

The JSON repositories can be found in the 'JsonRepositories' folder found at the root of the project (this application is only a sample so none of the data is encrypted or hidden). Inside is some sample data created through testing. Please feel free to log into any account from the JSON and create your own as well.

*Please do not directly alter the JSON in a way that will change the object structure or go against the JSON format. The application will be sad if it can't read the JSON files. :(

If there are any questions or concerns about the source code or functionality of the applications please feel free to contact me. If the github repository does not work correctly I will be happy to zip up my local directory and send it directly.

Thanks,
Caleb Kauffman