# SCI-Server

**SCI = System Control Interface**.
The program serves as a server for the SCI. The program can be used to save data such as user data and control data. (See schematic)

## Description

This is a server-side application that is the counterpart to the client. The program is a console application and can therefore be started on the server without the need for a graphical user interface.
The server has several start arguments, such as an integrated test module. This allows you to quickly test whether the actual application is working or not.
The server will create its own file system with the required data directly at startup.
A config.json is created for settings on the server. This can be changed.
SQLite is used to save data. A system user (root) is created directly here. This is protected from all changes so that there is always a way to get into the system if you have locked yourself out. Only a new password for root can be changed.
The program can use several modules, such as an RS232 (serial connection), which executes certain commands to control machines and electronics.
The server and client communicate via a TCP connection that is established synchronously. The messages are encrypted and decrypted on the other side with SHA512. 

## Getting Started

### Dependencies

* You can build this program on you own with *dotnet publish* 
* Tested on Linux-x64 and Windows-x64

### Installing

* How/where to download your program
* Any modifications needed to be made to files/folders

### Executing program

#### Windows

* Run Server
```
./SCI-Server.exe
```

* Run Server with Debug mode
```
./SCI-Server.exe --debug
```

* Run Test Module
```
./SCI-Server.exe --test
```

#### Linux

* Run Server
```
./SCI-Server
```

* Run Server with Debug mode
```
./SCI-Server --debug
```

* Run Test Module
```
./SCI-Server --test
```

## Help

Any advise for common problems or issues.
```
command to run if program contains helper info
```

## Authors


## Version History

* 0.2
    * Various bug fixes and optimizations
    * See [commit change]() or See [release history]()
* 0.1
    * Initial Release

## License

This project is licensed under the MIT License - see the LICENSE.md file for details
