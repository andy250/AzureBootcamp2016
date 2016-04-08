#!/usr/bin/python

import AzureIOTHUB
import random
import time

KEY = "###";
HUB = "###";
DEVICE_NAME = "###";

device = AzureIOTHUB.DeviceClient(HUB, DEVICE_NAME, KEY)
device.create_sas(1200)

t = 28
p = 1013

# Device to Cloud
while True:
    t = t + random.uniform(-0.2, 0.2)
    p = p + random.uniform(-1, 1)
    msg = b"{{message: 'Hello from Python Sim!', temp: {0}, pressure: {1}, epoch: {2} }}".format(t, p, int(time.time()))
    print msg
    print(device.send(msg))
    time.sleep(2)