# RUDP
a weak test
当你作为服务器接收一个来自于任何远端的消息时
判断他是否已存在与我们的用户列表里
未存在的话
新建 RUDP类 用于与该用户进行通讯
RUDP里包含RReceiver和RSender两个工具类，用来处理消息的次序与重发确认等
