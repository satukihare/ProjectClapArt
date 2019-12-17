# Expression
# 指定のキャラの表情を変化させる

# 引数
# num - 変化させるキャラの指定
# 0で左、1で右
# exp - 変化先の表情の指定
# 任意の値

[Expression]
char
exp


# Face
# 指定のキャラの顔色を変化させる

# 引数
# num - 変化させるキャラの指定
# 0で左、1で右
# face - 変化先の顔色の指定
# 任意の値

[Face]
char
face


# Sound
# 音声(ボイスor効果音)を再生する

# 引数
# sound - 再生する音声のID

[Sound]
sound


# Speech
# テキストを表示させる

# 引数
# char - 喋るキャラ
# 0で左、1で右、-1でなし（地の文)
# text - 表示するテキスト(n行まで)

[Speech]
char
text
...
text

# Banner
# バーとテキストを表示させる

# 引数
# text - 表示するテキスト（1行）

[Banner]
text

# Fade
# フェード演出を再生させる

# 引数
# id - フェード演出の種類

[Fade]
id

# End
# ノベルパートを終了させる

[End]



# 使用例

[Expression]
0
0

[Sound]
0

[Speech]
0
私だ

[Expression]
1
1

[Face]
1
2

[Speech]
1
お前だったのか

[End]