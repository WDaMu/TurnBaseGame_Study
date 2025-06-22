### 3.1

- 素材文件转URP：使用素材中内置的UPR提取器将素材升级为UPR（使用Unity中内置的渲染管道转换器（Render Pipeline  Converter）无法无法完成转换，暂时不明白问题出在哪）
- 添加后处理（Post-processing），通过调整场景中的Global Volume（设置后处理文件和添加组件）来实现后处理效果的添加。
- 创建静态工具类管理鼠标（使用Physics.Raycast函数获取鼠标点击位置）
- Unity中Layer的获取（通过LayerMask.GetMask函数和位移操作获取）
- 人物移动（使用Vector3.MoveTowards函数实现）
- 导入动画资源
  - 将动画类型设置为人形（Humanoid）并从该模型创建骨骼
  - 更改动画片段名称并将动画设置为循环
  - 矫正动画的模型方向，将Root Transform Rotation中的Based Upon设置为Original
- 为人物添加枪械并调整枪械的动画（重新录制动画），对于在Unity外部创建的动画，无法直接修改，需要复制一份再进行修改。

### 3.2
- 添加动画控制器，实现人物移动时的动画切换
- 添加选择被选中的Unit的显示
- 编写基础的网格系统，限制人物在网格系统内移动
- 网格显示Prefab的制作






