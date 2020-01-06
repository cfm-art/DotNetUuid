Uuid
==

.NetでUUIDを生成する。

## UUID version1
タイムスタンプと端末識別子 (MACアドレス) を利用したUUID。

```cs:v1usage.cs

using CfmArt.Uuid;

var uuid1 = Uuid.Timestamp();

// -- or --

var uuid2 = Uuid.V1.Generate();

```

## UUID version2
今のところ無し

## UUID version3
今のところ無し

## UUID version4
乱数を利用したUUID。

とりあえず、System.Guidを利用。

```cs:v4usage.cs

using CfmArt.Uuid;

var uuid1 = Uuid.Random();

// -- or --

var uuid2 = Uuid.V4.Generate();

```

## UUID version5
今のところ無し
