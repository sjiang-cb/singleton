/*
 * Copyright 2020 VMware, Inc.
 * SPDX-License-Identifier: EPL-2.0
 */

package main

import (
	"testing"

	. "github.com/smartystreets/goconvey/convey"
	sgtn "github.com/vmware/singleton"
)

func TestGetConfigFromInstance(t *testing.T) {
	Convey("Instance is available, config can be got(P0)", t, func() {
		cfPath := "testdata/Config/config.json"
		testcfg, _ := sgtn.LoadConfig(cfPath)
		sgtn.Initialize(testcfg)
		// gotconfig := inst.GetConfig()

		// So(gotconfig, ShouldEqual, *testcfg)

	})

	// Convey("Instance isn't available, config can't be got(P0)", t, func() {
	// 	cfPath := "testdata/Config/configProductEmpty.yaml"
	// 	testcfg, _ := sgtn.NewConfig(cfPath)
	// 	testinst, _ := sgtn.NewInst(*testcfg)
	// 	gotinst, ok := sgtn.GetInst(testcfg.Name)

	// 	// So(loaded, ShouldBeFalse)
	// 	So(testinst, ShouldNotBeNil)
	// 	So(ok, ShouldBeTrue)
	// 	So(gotinst, ShouldEqual, testinst)
	// })
}
