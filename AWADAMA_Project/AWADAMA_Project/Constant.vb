Public Module Constant

    Public ReadOnly Property PLAYER_NO_1 As Integer = 0
    Public ReadOnly Property PLAYER_NO_2 As Integer = 1

    Public ReadOnly Property MAX_DECK_COUNT As Integer = 30

    Public ReadOnly Property MAX_CARD As Integer = 37

    Public ReadOnly Property MAX_CARD_CELL As Integer = MAX_CARD + 1

    Public ReadOnly Property HandCardNum As Integer = 6

    Public ReadOnly Property AllyCardNum As Integer = 6

    Public ReadOnly Property EnemyCardNum As Integer = 6

    Public ReadOnly Property UpdateDisplayMillSeconds As Integer = 500

#Region "スプレッドシート関連定数"

    Public ReadOnly Property MAIN_SHEET_ID As String = "1ysmqUCvlxr0neGqNpzNnpdVpG4nsENdOzggoQ5ZLTOw"

    Public ReadOnly Property MAIN_SHEET_NAME_DECK As String = "カード一覧"
    Public ReadOnly Property MAIN_SHEET_DECK_RANGE As String = "B2:B" & Constant.MAX_CARD_CELL

    Public ReadOnly Property MAIN_SHEET_NAME_ARCHIVE As String = "アーカイブ"
    Public ReadOnly Property MAIN_SHEET_ARCHIVE_RANGE As String() =
        {"A2:C" & Constant.MAX_CARD_CELL, "G2:I" & Constant.MAX_CARD_CELL}
    Public ReadOnly Property MAIN_SHEET_ARCHIVE_CARD_NAME_RANGE As String() =
        {"B2:B" & Constant.MAX_CARD_CELL, "H2:H" & Constant.MAX_CARD_CELL}
    Public ReadOnly Property MAIN_SHEET_ARCHIVE_CARD_FRAW_FLG_COLUMN As String() =
        {"C", "I"}
    Public ReadOnly Property MAIN_SHEET_GRAVE_START_CELL As String() =
        {"E2", "K2"}

    Public ReadOnly Property MAIN_SHEET_NAME_BATTLE As String = "バトル"
    Public ReadOnly Property MAIN_SHEET_BATTLE_ALL_DATA As String =
        "A1:G31"
    Public ReadOnly Property MAIN_SHEET_BATTLE_UPDATE_FLGE_CELL As String() =
        {"A21", "A1"}
    Public ReadOnly Property MAIN_SHEET_BATTLE_HAND_START_CELL As String() =
        {"B20", "B2"}
    Public ReadOnly Property MAIN_SHEET_BATTLE_AREA_START_CELL As String() =
        {"B13", "B8"}
    Public ReadOnly Property MAIN_SHEET_BATTLE_ENWMY_HAND_RANGE As String() =
        {"B2:G11", "B20:G20"}
    Public ReadOnly Property MAIN_SHEET_BATTLE_ENWMY_AREA_RANGE As String() =
        {"B8:G8", "B13:G13"}

#End Region

End Module
