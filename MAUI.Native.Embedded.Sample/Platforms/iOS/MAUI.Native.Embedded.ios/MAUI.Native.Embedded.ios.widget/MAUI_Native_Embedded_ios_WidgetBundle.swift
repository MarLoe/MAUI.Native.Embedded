//
//  MAUI_Native_Embedded_ios_WidgetBundle.swift
//  MAUI.Native.Embedded.ios.Widget
//
//  Created by Martin Løbger on 31/07/2025.
//

import WidgetKit
import SwiftUI

@main
struct MAUI_Native_Embedded_ios_WidgetBundle: WidgetBundle {
    var body: some Widget {
        MAUI_Native_Embedded_ios_Widget()
        MAUI_Native_Embedded_ios_WidgetControl()
        MAUI_Native_Embedded_ios_WidgetLiveActivity()
    }
}
