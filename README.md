# CopyShit
Tool for copying files reliably to slow media on Windows 10/11

Why does this program exist? Basically, I was having weird glitches and explorer.exe 
crashed while I was trying to copy files to a blu-ray drive formatted as UDF using packet-writing mode. 
So I initially wrote a short program in perl to diagnose the issue and found that no problems occurred
when I read and wrote to files with read() and write() functions, even writing to random places in a file-- 
it was just slow, which one would expect from optical disks. I still don't really know what makes 
Explorer.exe so bad at writing to blu-ray disks, but in any case.

This program is works to copy files, esp large files like zip archives, to slow media like blu-ray disks,
which explorer.exe fails at for some reason.

