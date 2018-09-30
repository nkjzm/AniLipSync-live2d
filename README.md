# AniLipSync-live2d

Live2Dでリミテッドアニメのようなリップシンクを実現するためのライブラリです。

![](docs/sample.gif)

[AniLipSync](https://github.com/XVI/AniLipSync)の実装がベースになっています。
また、VisemeShapeClipの実装で[UniVRM](https://github.com/dwango/UniVRM)を参考にしています。

## Download

[AniLipSync-live2d v1.0.0](https://github.com/nkjzm/AniLipSync-live2d/releases/download/v1.0.0/AniLipSync-live2d.unitypackage)

## Environment
- Windows 10
- Unity 2018.2.10f1

## Requirement

- [OVRLipSync Version 1.28.0](https://developer.oculus.com/downloads/package/oculus-lipsync-unity/1.28.0/)
- [Live2d Cubism 3 SDK for Unity R9](https://live2d.github.io/#unity)

## Samples

`Assets/AniLipSync-live2d/Examples/Scenes/AniLipSync-live2d.unity`

## Getting Stared

1. Open your unity project
1. Import OVRLipSync
1. Import Live2d Cubism 3 SDK for Unity
1. Import AniLipSync-live2d.unitypackage
1. Add prefab, `Assets/Oculus/LipSync/Prefabs/LipSyncInterface` in the scene
1. Add prefab, `Assets/AniLipSync-live2d/Prefabs/AniLipSync-live2d` in the scene
1. Create 5 VisemeShapeClips, `aa`, `E`, `ih`, `oh`, `ou`
1. Set `AnimMorphTarget` component in `AniLipSync-live2d` prefab 

### VisemeShapeClip

![](docs/clip-setting.png)

// write later

# License

This library under [MIT License](LICENSE)

This library is derived from [AniLipSync](https://github.com/XVI/AniLipSync/blob/master/LICENSE)
