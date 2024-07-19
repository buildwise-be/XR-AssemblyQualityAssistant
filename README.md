# Demo-HL2-AssemblyQualityAssistant
Hololens2 demo showcasing some AEC applications

This application showcases 2 demos:

- An assembly assistant, made for Solredo.
- A quality inspection assistant, made for Schelfhout.

## Solredo Assembly Assistant
1. See the miniature model of the house to renovate
2. Select one of the modules to build
3. Scan a QR code to make the 1st submodule appear (use the QR code provided in this repo)
4. Use vocal command 'Next' to make appear subsequent modules
5. Get current step information from hand menu
6. Acknowledge the step went fine, or mention problems if any

## Schelfhout Quality Assistant
1. 'Click' with your fingers to make the concrete slab appear
2. Adjust its position (overlay it to the real slab)
3. Fine-tune its position
4. Lock its position
5. Start the quality review (hooks)

## Build instructions
1. Build for Universal Windows Platform in Unity
2. Open the built project in Visual Studio 2022
3. Build for Master ARM64
4. Deploy to Hololens2

## Acknowledgments :pray:
Based on the work of [Joost van Schaik](https://github.com/LocalJoost).

## Remarks
Using Unity 2021.3.23f1, MRTK3, and other build settings as described [here](https://github.com/LocalJoost/QRCodeService/tree/serviceframework).
