# CustomMinecraftLauncher

a new minecraft launcher for forge, CMLauncher Pro Canceled.

Currently maintained

# Questions

 - Q: Is This Open Source?
 - A: No.

 - Q: Will This Always Be Free?
 - A: Yes.

 - Q: Can It Download Any Version?
 - A: Mostly

# Tasklist

- [x] Login To Minecraft
- [x] Download Minecraft Versions
- [x] Install Forge
- [x] Mod Profiles
- [ ] Launch Shortcuts (WIP)

# Launch Shorcuts v1

To Make A Shortcut Make Sure That you make a new .json file with the syntax below then make a new shortcut then The location is "Game Launcher.exe Location" "JSON File Location" (Old Versions)

Go into the modding tab and click "Add .cmlaunch file association", this does require administrator. the old way will still work. (Not Public Yet)


![image](https://user-images.githubusercontent.com/67196220/116825912-66cebe00-ab5f-11eb-8b1c-7fc96584f1b5.png)
![image](https://user-images.githubusercontent.com/67196220/116825997-de9ce880-ab5f-11eb-9e70-d47625dda087.png)


 - [ ] Login (Not Possible)
 - [x] Version
 - [x] Memory
 - [ ] Mod Profile On Launch

# Syntax 
{"$schema":"https://raw.githubusercontent.com/The-Developer-Nate/JSONSchema/main/CustomMinecraftLauncher.json", "email": "example@example.com","password": "ExamplePass","version": "1.16.5","memory": "4096","offlineuser": "OfflineUser","mpn": "ModProfile Name", "timelimit": -1}
