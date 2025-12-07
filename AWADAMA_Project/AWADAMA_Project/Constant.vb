Public Module Constant

    Public ReadOnly Property MAX_DECK_COUNT As Integer = 30

    Public ReadOnly Property MAX_CARD As Integer = 37

    Public ReadOnly Property MAX_CARD_CELL As Integer = MAX_CARD + 1

    Public ReadOnly Property HandCardNum As Integer = 6

    Public ReadOnly Property AllyCardNum As Integer = 6

#Region "スプレッドシート関連定数"

    Public ReadOnly Property MAIN_SHEET_ID As String = "1ysmqUCvlxr0neGqNpzNnpdVpG4nsENdOzggoQ5ZLTOw"

    Public ReadOnly Property MAIN_SHEET_NAME_DECK As String = "カード一覧"

    Public ReadOnly Property MAIN_SHEET_NAME_ARCHIVE As String = "アーカイブ"

    Public ReadOnly Property MAIN_SHEET_NAME_BATTLE As String = "バトル"

#End Region

End Module
