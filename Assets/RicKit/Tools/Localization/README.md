使用说明:

1. 使用前请先自行导入Newtonsoft.Json库。可在PackageManager处，通过git URL，输入"com.unity.nuget.newtonsoft-json"导入。
2. 可以自己继承IJsonConverter实现新的Json转换工具；然后在Config中选择自己的Converter
3. 如果需要自定义功能，请在继承了LocalizationPackageAbstractEditor的类中重写OnInspectorGUI方法
4. 如果需要创建MainEditor，请从Menu中选择RicKit -> Localization -> Create MainEditor
5. 使用“批量XXX”功能前请先配置"Localization/Config.asset"设置游戏将会支持的所有语言（如果有未包含的语言，自行修改"RicKit.Tools.Localization.LanguageEnum"）
6. 可以修改LanguageEnum为自己的语言枚举；该枚举影响输出json名字，请务必再一开始就改好
7. 翻译需要导入Selenium.unitypackage；并且安装对应版本的浏览器driver，下载后放置在Plugins/Selenium.WebDriver.4.15.0/drivers；并在Config中设置自己的Driver
8. 使用Edge浏览器的请看https://learn.microsoft.com/zh-cn/microsoft-edge/webdriver-chromium/?tabs=c-sharp
9. 使用Chrome浏览器的请看https://blog.csdn.net/Z_Lisa/article/details/133307151
10. 使用FireFox浏览器的请看https://blog.csdn.net/weixin_39339460/article/details/113773738
