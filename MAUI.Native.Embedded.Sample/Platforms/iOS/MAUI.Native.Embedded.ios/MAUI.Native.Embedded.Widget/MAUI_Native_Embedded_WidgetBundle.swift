//
//  MAUI_Native_Embedded_WidgetBundle.swift
//  MAUI.Native.Embedded.Widget
//
//  Created by Martin Løbger on 31/07/2025.
//

import WidgetKit
import SwiftUI

@main
struct MAUI_Native_Embedded_WidgetBundle: WidgetBundle {
    var body: some Widget {
        MAUI_Native_Embedded_Widget()
        MAUI_Native_Embedded_WidgetControl()
        MAUI_Native_Embedded_WidgetLiveActivity()
    }
}
