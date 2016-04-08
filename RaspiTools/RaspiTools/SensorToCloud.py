#!/usr/bin/python

import Adafruit_BMP.BMP085 as BMP085
import AzureIOTHUB
import random
import time

KEY = "###";
HUB = "###";
DEVICE_NAME = "###";

device = AzureIOTHUB.DeviceClient(HUB, DEVICE_NAME, KEY)
device.create_sas(1200)

sensor = BMP085.BMP085()

print 'Temp = {0:0.2f} *C'.format(sensor.read_temperature())
print 'Pressure = {0:0.2f} Pa'.format(sensor.read_pressure())
print 'Altitude = {0:0.2f} m'.format(sensor.read_altitude())
print 'Sealevel Pressure = {0:0.2f} Pa'.format(sensor.read_sealevel_pressure())

while True:
    t = sensor.read_temperature()
    p = sensor.read_pressure()
    msg = b'{{"message": "Hello from Python Sensor!", "temp": {0:0.2f}, "pressure": {1:0.2f}, "epoch": {2} }}'.format(t, p / 100.0, int(time.time()))
    print msg
    print(device.send(msg))
    time.sleep(2)