# MIS
本系统只可在华中科技大学管理学院239实验室运行，用到了固定式读写器，桌面式读写器（可选），zigbee定位

为了实现停车管理的智能化、车辆进出高效化，我们团队应用大数据实验室硬件设备、电子标签与自行开发的软件系统，模拟停车场环境、车辆进出、车位管理，通过详细设计、不断完善，让整个系统达到可以应用到实际中、为所有停车场管理、用户使用带来便利的目的。

该停车场用户根据临时停车和长期停车分为两类，根据停在固定车位和公共车位分为两类，总结成三类用户：固定车位固定用户（长期停车在固定车位）、固定车位临时用户（临时停车在固定车位）、公共车位普通用户（临时停车在公共车位）。

桌面式读卡器读取RFID卡号，也可手动输入。

固定式读写器读取用户进出车库的时间，保存到数据库，用于临时停车的计费（针对固定车位临时用户和公共车位普通用户）。

zigbee定位用于停在固定车位的车辆定位。

![image](https://github.com/chancechang/MIS/raw/master/image/main.png)

如上面两图所示。界面分为五部分，上边的长条的功能栏中列举了固定车位管理、停车管理、收费管理、人员维护、分析查询、切换账号等6种系统功能。中间的图标表示停车场的固定车位和公共车位，每个标着数字的小方块都代表一个车位，若小方块为绿色，则表明当前车位未被占用，若小方块的颜色为红色，则表明当前车位已经有车辆停放。下方的方框用来显示分析查询的结果，包括付费记录查询、收入报表和车位使用率。
右侧上方是管理员当登录状态显示，通过此处显示管理员类别（“超级”、“系统”）与姓名，同时管理员还可以通过“退出”按钮，退出登录；右侧下方为车位情况，显示当前时间，以及公共车位、固定车位的使用情况。


