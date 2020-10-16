# 尝试使用Abp vNext 构建我的商城 (一) :一个简单的商城项目


## 前言

### 此文目的

本文将尝试使用Abp vNext 搭建一个商城系统并尽可能优化升级项目结构，并会将每次升级所涉及的模块及特性以blog的方式展示出来。

### Abp vNext 介绍

#### 什么是Abp vNext

## 代码实践

> Abp vNext 使用 DDD 领域驱动设计 是非常方便的，但是由于本人认为自身没有足够的功力玩转DDD，所以开发中使用的是基于贫血模型的设计的开发，而不是遵照DDD的方式

### Domain

#### 添加引用

 > 通过 **Nuget** 安装 **Volo.Abp.Ddd.Domain**

#### 定义模块

创建 **MyShopDomainModule.cs** 作为Domain的模块，Domain不依赖任何外部模块，本体也没有什么相关配置，所以只需要继承AbpModule即可

#### 定义实体

###### BaseEntity 实体基类

定义BaseEntity并继承由**Volo.Abp.Ddd.Domain**提供的Entity并添加**CreationTime**属性


###### Product 商品

继承**BaseEntity**类 添加相关属性

###### Order 订单

继承**BaseEntity**类 添加相关属性

### Application



### Repository



### Migration


 
### Api





