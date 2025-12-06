Public Class HandDataDto
    Private value As Object
    Public Property No As String
    Public Property Name As String
    Public Property Draw As String

    ' -----------------------------------------------------------------
    ' コンストラクター（値を設定するための初期化処理）
    ' -----------------------------------------------------------------
    Public Sub New(ByVal no As String _
            , ByVal name As String _
            , ByVal draw As String)

        Me.No = no
        Me.Name = name
        Me.Draw = draw
    End Sub

    Public Sub New()

    End Sub
End Class
