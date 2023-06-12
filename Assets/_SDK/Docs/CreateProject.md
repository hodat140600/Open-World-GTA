## Environments
- Development Environment: Unity Editor Version 2021.3.6f1.

NOTE: Chỉ được dùng version trong được cung cấp trong link download phía dưới
- [Max SDK](https://nextcloud.rocketstudio.com.vn/s/mWtGftaDDBKgxXC) : 5.5.7
- Firebase : 9.1.0
    [FirebaseAnalytics](https://nextcloud.rocketstudio.com.vn/s/APsdCLd5ZWtNMTQ),
    [FirebaseCrashlytics](https://nextcloud.rocketstudio.com.vn/s/Dff8pCfKrRRzagz),
    [FirebaseRemoteConfig](https://nextcloud.rocketstudio.com.vn/s/EFpZRzyLXs462pX).
- [AppsFlyer](https://nextcloud.rocketstudio.com.vn/s/QTA9PdDoNCWczpC) : 6.8.1

- Build Environment: JDK, NDK, Gradle - Đọc hướng dẫn [Publish.md](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Docs/Publish.md)

## Check List khi Tạo Unity Project
NOTE: 
- **Chú ý các versions** vì nếu sai sẽ tốn rất nhiều thời gian cho build
- **Uncheck folder ExternalDependencyManager** khi import các package vì trong SDK đã giữ phiên bản cao nhất.
![image](https://user-images.githubusercontent.com/117144985/200244712-02ccf339-8c06-4d2e-87ff-8cac30b771ae.png)

### Steps
- Fork RocketSgSdk Repository cho Game mới và Clone về máy local.
- Cài các thư viện sau:
  + **Max (Applopvin)**:
    Import Package [Max package](https://nextcloud.rocketstudio.com.vn/s/mWtGftaDDBKgxXC) 
    
    Disable Max: Khi start một game chúng ta chưa có key từ Marketing vì vậy phải disable Max cho đến khi cài đặt Ads.
    
    Các Ads Network (Facebook, Unit Ads, etc) cũng chỉ được install khi có thông tin từ bên MKT, đọc file [Ads.md](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/Docs/Ads.md) để biết thêm chi tiết. 
    
    ![image](https://user-images.githubusercontent.com/117144985/200223627-a2c3b63b-d12e-4a7a-ac11-16b72bbede9f.png)
    
  + **AppsFlyer**:
    Import Package [AppsFlyer package](https://nextcloud.rocketstudio.com.vn/s/QTA9PdDoNCWczpC).
    NOTE: Hiện tại đang Disable Appflyer, cần cập nhật ID,Key prefab này trong LoadingScene khi bên Marketing đưa.
    
    <img width="413" alt="image" src="https://user-images.githubusercontent.com/117144985/200230500-864dbcf9-18aa-493d-88d0-517ff0e58f4c.png">

   + **FireBase** :
    
    Chỉ Import các Packages này:
     
     [FirebaseAnalytics](https://nextcloud.rocketstudio.com.vn/s/APsdCLd5ZWtNMTQ),
     
     [FirebaseCrashlytics](https://nextcloud.rocketstudio.com.vn/s/Dff8pCfKrRRzagz),
     
     [FirebaseRemoteConfig](https://nextcloud.rocketstudio.com.vn/s/EFpZRzyLXs462pX). 
    
    NOTE: Hiện tại đang dùng file google-services của SDK, cần cập nhật file này khi bên Marketing đưa. Contact Thức, Hiếu để lấy access vào Firebase Dashboard Test

- Run Android Resolver : Assets ->External Dependency Manager -> Android Resolver -> Force Resolver. 
<img width="413" alt="image" src="https://user-images.githubusercontent.com/117144985/200237572-22b2c62c-b1c2-4fb1-ac59-82267a045c83.png">

  + NOTE: Đợi đến khi chạy xong, không sử dụng đến Gradle trong quá trình này VD: build project khác , Android Resolver project khác....Lý do vì Java Env share chung một gradle cache.

- Build APK để test quá trình Setup, check Player Settings -> Other Settings để biết thêm chi tiết các Setup. Các setup này sẽ được chỉnh lại khi có thêm thông tin tử MKT
  ![image](https://user-images.githubusercontent.com/117144985/200224887-e0da9681-647f-4705-85f6-e00938322ed9.png)

### Các đoạn code nên đọc để hiểu (Optional)
- [LoadingScene.cs](https://github.com/rocketsaigon/RocketSgSdk/blob/master/Assets/_SDK/SplashScreen/LoadingScene.cs) - Để load các Services cần thiết.

### Sau khi Import UnityPackage ở bước trên, các Packages nhớ kiểm tra các Packages sau:

#### Code Pattern Package
- [UniRx](https://assetstore.unity.com/packages/tools/integration/unirx-reactive-extensions-for-unity-17276#description)
- [State Machine](https://github.com/dotnet-state-machine/stateless)


#### Dev Utilities Packages
- [NugetForUnity](https://github.com/GlitchEnzo/NuGetForUnity/releases)
- [Joystick Pack](https://assetstore.unity.com/packages/tools/input-management/joystick-pack-107631/reviews)
- [Find Reference 2](https://nextcloud.rocketstudio.com.vn/s/sAk6XP38CZzPAf7?path=%2FVietlabs%2FEditor%20ExtensionsUtilities)
- [Odin](https://nextcloud.rocketstudio.com.vn/s/sAk6XP38CZzPAf7?path=%2FSirenix%2FEditor%20ExtensionsSystem)
