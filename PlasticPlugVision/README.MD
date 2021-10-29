SUBSYSTEM=="vchiq",MODE="0666"

In order to grant all users access to the Raspberry Pi Camera, create the file /etc/udev/rules.d/99-camera.rules with the following content:

SUBSYSTEM=="vchiq",MODE="0666"
It's common practice to prefix udev rule files with a number. This is because udev rules are executed in alphabetical order, and using a number prefix allows us to control that order. We're using "99" because we want the built-in rules to execute before our custom rule.

The rule we added looks very similar to the built-in rule, with two exceptions:

The "GROUP" statement is removed so the camera can be accessed by users outside of the "video" group.
The "MODE" is changed from "0660" to "0666". Changing the last number from a "0" to a "6" grants read/write access to all users. Feel free to read up on Linux permissions to learn more about how that works.
All Docker containers will now have access to the camera device. The device can be passed into the container using Docker's --device flag:

--device /dev/vchiq