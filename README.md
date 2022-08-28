# iEra Upload 
[![License](https://img.shields.io/badge/license-Apache-blue)](./LICENSE) ![Platform](https://img.shields.io/badge/platform-windows-lightgrey)

### Preamble
This software is an unofficial, individually-developed standalone for GraalOnline Era's customs uploader, which you can find [here](https://eraupload.graalonline.com). This software is in no way affiliated with GraalOnline or any other registered trademarks of Toonslab.

### Technical Specifications
- **.NET:** 6.0 (long-term support)
- **CefSharp.WinForms.NETCore:** 104.4.240

### Frequently Asked Questions
**Q:** Why did you create this?
<br>
**A:** Primarily, because I wanted something more convenient to submit custom uploads with. I always disliked needing to open a separate browser/bookmark for it. Second, I wanted to have fun creating something for a game I genuinely enjoy playing.

**Q:** Why can I not adjust the size of the window (including minimizing & maximizing)?
<br>
**A:** It would malform the display of the uploader's contents, and I am too lazy to currently fix it. Besides, everything the uploader requires of you prior to submitting your graphic is present from the start.

**Q:** Will there be a standalone uploader for the other mobile Graal servers?
<br>
**A:** I currently have no plans to implement a standalone uploader for the other mobile Graal servers. If you would like one, feel free to download and extract this repository, then open the Visual Studio solution and assign the following constant's value in [App.cs](./iEra%20Upload/App.cs) to the correct customs uploading site for your server.
```c# 
private readonly string Url = "https://eraupload.graalonline.com"
```
Once you have done so, build the solution or project and run it.

**Q:** Will this repository be maintained?
<br>
**A:** I will do my best at keeping this repository maintained and up-to-date but don't take my word for it.

**Q:** I found a bug or want to report an issue. How do I do so?
<br>
**A:** Contact me on Discord at Crane#3815.

### Screenshots
![Screenshot](/iEra%20Upload/Resources/Capture.PNG)

### Acknowledgements
Listed below are entites that have in some way shape or form assisted with the development of this software.
- Toonslab
- GraalOnline Era Staff & Community
- Spolsh
- Conner
- Mythric
