Public Module Constant

    Dim const_max_card As Integer = 37
    Public ReadOnly Property MAX_CARD As Integer
        Get
            Return const_max_card
        End Get
    End Property

    Dim const_max_card_cell As Integer = const_max_card + 1
    Public ReadOnly Property MAX_CARD_CELL As Integer
        Get
            Return const_max_card_cell
        End Get
    End Property

    Dim const_hand_card_num As Integer = 6
    Public ReadOnly Property HandCardNum As Integer
        Get
            Return const_hand_card_num
        End Get
    End Property

#Region "スプレッドシート関連定数"

    Dim const_main_sheet_id As String = "1ysmqUCvlxr0neGqNpzNnpdVpG4nsENdOzggoQ5ZLTOw"
    Public ReadOnly Property MAIN_SHEET_ID As String
        Get
            Return const_main_sheet_id
        End Get
    End Property

    Dim const_main_sheet_name_deck As String = "カード一覧"
    Public ReadOnly Property MAIN_SHEET_NAME_DECK As String
        Get
            Return const_main_sheet_name_deck
        End Get
    End Property

    Dim const_main_sheet_name_archive As String = "アーカイブ"
    Public ReadOnly Property MAIN_SHEET_NAME_ARCHIVE As String
        Get
            Return const_main_sheet_name_archive
        End Get
    End Property

#End Region

End Module
