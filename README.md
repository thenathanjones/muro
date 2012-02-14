What is it?
===========
Muro is a .NET build monitoring tool that is accessible via a web browser.  

Why do this?
============
Why not?  I've toyed with a couple of different tools, written in Scala and Ruby but I wanted a .NET version to tie in with the other tools I've been working on, [NUSB](https://github.com/thenathanjones/nusb) and [Ojo](https://github.com/thenathanjones/ojo).

Surely this exists?
===================
If it does, Google is having trouble finding it at of the 18th of September 2011.  I've used greenscreen.rb (Ruby) and BigVisibleWall (Scala) but wanted a .NET version to tie in with the parsing library I'd written, [Burro](https://github.com/thenathanjones/burro).

How do I use this?
==================
Pre-requisites
--------------
I've built Muro as a .NET 4 project, so the runtime will need to be installed.
Configuration
-------------
On startup, Muro will look for a configuration file in the installed directory called "muro.yml".  If one isn't present, it will create one with a sample configuration and close.  You may supply the full path to an alternative config file as an argument to the service. 
Here is an example config:

     # Configuration file for Muro build monitoring tool
     -
       servertype: CruiseControl
       url: http://10.1.1.2:8153/go/cctray.xml
       username: 
       password: 
       pipelines:
         -
           name: "Trunk :: spec"
           
At this point in time it's the standard [Burro](https://github.com/thenathanjones/burro) configuration.  Check that project for details.
It will only read the configuration file on startup, so if you make any changes to the file, you will need to restart the service to pick them up.
Running Muro
-------------
The intention is to run Muro as a Windows service which is how it is installed.  It can also run as a standalone file just by running the executable.  If any errors occur when running it, they will appear in the Windows Event log.
Usage
-----
Fire up a web browser, and browse to port 4567.  Make the browser full screen and you'll have a monitor over your monitored builds.

Limitations
===========
The types of build servers supported are again limited by whatever [Burro](https://github.com/thenathanjones/burro) is capable of, and as such check that project for what you can use.