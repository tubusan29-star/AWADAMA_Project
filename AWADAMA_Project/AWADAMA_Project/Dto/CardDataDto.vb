Public Class CardDataDto
    Private value As Object
    Public Property DeckNo As String
    Public Property No As String
    Public Property Name As String
    Public Property Cardname As String
    Public Property Cost As String
    Public Property Type As String
    Public Property Line As String
    Public Property Atack As String
    Public Property Defense As String
    Public Property Intellect As String
    Public Property Magic As String
    Public Property Effect As String
    Public Property Flavor As String
    Public Property Skill1 As String
    Public Property Skill2 As String
    Public Property Skill3 As String

    ' -----------------------------------------------------------------
    ' コンストラクター（値を設定するための初期化処理）
    ' -----------------------------------------------------------------
    Public Sub New(ByVal no As String _
            , ByVal name As String _
            , ByVal cardname As String _
            , ByVal cost As String _
            , ByVal type As String _
            , ByVal line As String _
            , ByVal atack As String _
            , ByVal magic As String _
            , ByVal defense As String _
            , ByVal intellect As String _
            , ByVal effect As String _
            , ByVal flavor As String _
            , ByVal skill1 As String _
            , ByVal skill2 As String _
            , ByVal skill3 As String)

        Me.No = no
        Me.Name = name
        Me.Cardname = cardname
        Me.Cost = cost
        Me.Type = type
        Me.Line = line
        Me.Atack = atack
        Me.Magic = magic
        Me.Defense = defense
        Me.Intellect = intellect
        Me.Effect = effect
        Me.Flavor = flavor
        Me.Skill1 = skill1
        Me.Skill2 = skill2
        Me.Skill3 = skill3
    End Sub

    Public Sub New()

    End Sub
End Class
