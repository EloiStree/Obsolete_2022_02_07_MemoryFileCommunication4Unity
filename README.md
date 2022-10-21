# 2022_02_07_MemoryFileCommunication4Unity
  
Use in my project to make communication between two Unity Application through RAM file.  
It works for Mono project but don't for IL2CPP.  

If you know how to make it work for IL2CPP and want to help:    
Contact me :) Please.    



# Obsolete  
  
This project was design for a Kinect project done for ActiveMe.  
It was not working under IL2CPP and outside of Unity.  
- So I design a version that are not using Mutex to make it works under IL2CPP.  
- And design a version in DLL format to make communication with outside app of Unity.  

You can still use it.
But I won't maintain it.

Find the next version: 
- DLL version to make it works outside Unity.
  - https://github.com/EloiStree/2022_10_20_MemoryFileConnectionUtilityDLL  
- Unity version based on the DLL to make it works in Unity
  - https://github.com/EloiStree/2022_10_19_MemoryFileCommunicationDllInUnity 
